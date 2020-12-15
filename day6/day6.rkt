#lang racket

(define (kwic-read filename)
  (with-input-from-file filename
    (λ ()
      (for/list ([line (in-lines)])
        line))))

(define input (kwic-read "1.txt"))

(define cs (append (map (λ (y) (string->list y)) input) (list '())))

(define (my-flatten lst)
  (cond ((null? lst) '())
        ((pair? lst)
         (append (my-flatten (car lst)) (my-flatten (cdr lst))))
        (else (list lst))))

(define count 0)
(define mem (make-hash))

(define (lm char) (map (λ (x)
     (cond ((= (hash-ref mem x 0) 1) 0)
           (else (hash-set! mem x 1) 1))) char))
(define mask (map (λ (l) (if (null? l) 0 1)) cs))
(define clist (map (λ (l m)
                     (if (= m 0) (hash-clear! mem) 0)
                     (lm l)
                     )
                   cs mask))
(define (lm2 char) (map (λ (x)
     (hash-set! mem x (+ (hash-ref mem x 0) 1)) 1) char))

(define (map-questions m c)
  (map (λ (x)
         (if (= x c) 1 0))
         (hash-values m)))

(define (all-questions m c)
  (apply + (map-questions m c)))

(define clist2 (map (λ (l m)
                     (if (= m 0)
                         (car (list
                               (all-questions mem count)
                               (set! count 0)
                               (hash-clear! mem)))
                         (car (list 0 (set! count (+ count 1)) (lm2 l)))))

                   cs mask))

(apply + (my-flatten clist))
(apply + (my-flatten clist2))

