# stack-script-0
Basic Stack-Oriented Scripting Language

## Dependency
.NET SDK, 6 or newer

## Build
```sh
dotnet build
```

## Run
view usage details
```sh
dotnet run
```
run unit tests
```sh
dotnet run -t
```
read-evaluate-print-loop (REPL)
```sh
dotnet run -repl
```

## Example
iterative Fibonacci Sequence
```sh
# 0 and 1 are starting values and the target index is 5
0 1 { i 0 } { index 5 } [ i index lo ] [ dup 2 rot + i 1 + { i } ] while
# view contents of stack
.
# 8 and 5 remain, with 8 on the top
8
5
```