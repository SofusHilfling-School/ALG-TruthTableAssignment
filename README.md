# ALG-TruthTableAssignment

# Second hand in - Logic

## A) Using a programming language of your choice write an application that generates a truth table for a given input – for example what is the truth table for ~(~p AND q) AND (p AND q)

## B) Using the laws of equivalence provide 10 examples of simplifying statements using the laws.

For this task we have made up 10 examples of statements that we will simplify using the laws.

### 1. ~(p ∧ ~q) ∨ p

We start by applying DeMorgan's law:

~(p ∧ ~q) ∨ p ≡ ~p ∨ ~~q ∨ p

Applying the double negative law doesnt really matter, as we can see that we can actually just apply the negation law to finish the simplification:

~p ∨ q ∨ p ≡ **t**


### 2. ~(~p ∧ ~q) ∨ ~(~p ∨ q) ∨ **t**

For this we can simply use the universal bound laws to see that this is just a tautology:

~(~p ∧ ~q) ∨ ~(~p ∨ q) ∨ **t** ≡ **t**

### 3. p ∨ (t ∨ ~q)

Similar to the one above, we first use the universal bound laws to isolate the tautology:

p ∧ (**t** ∨ ~q) ≡ p ∧ **t**

Now we can use the Identity law to remove the tautology from the statement:

p ∧ **t** ≡ p

### 4. (p ∨ (p ∧ q)) ∧ p

Here we first use the absorption laws:

(p ∨ (p ∧ q)) ∧ p ≡ p ∧ p

With this statement we can use the idempotent laws to isolate p:

p ∧ p ≡ p

### 5. ~(p ∧ ~q) ∨ ~(~p ∨ q)

### 6. (~p ∧ ~q) ∨ (~p ∨ q)

### 7. (~p ∧ ~q) ∨ (~p ∨ q)

### 8. (~p ∧ ~q) ∨ (~p ∨ q)

### 9. (~p ∧ ~q) ∨ (~p ∨ q)

### 10. (~p ∧ ~q) ∨ (~p ∨ q)
