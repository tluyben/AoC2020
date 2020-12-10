(define (kwic-read filename)
  (with-input-from-file filename
    (λ ()
      (for/list ([line (in-lines)])
        (string->number line)))))

(define input (kwic-read "1.txt"))

(apply * (filter (lambda (y) (member y
     (map (lambda (x) (- 2020 x))
          input))) input))