(define (kwic-read filename)
  (with-input-from-file filename
    (λ ()
      (for/list ([line (in-lines)])
        (string->number line)))))

(define input (kwic-read "1.txt"))

(apply * (filter (λ (y) (member y
     (map (λ (x) (- 2020 x))
          input))) input))