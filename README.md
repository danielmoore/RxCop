# RxCop

RxCop is a rule library that works with FxCop to provide additional 
checking for your observable sequences.

## Getting Started

To use RxCop, you will need to have installed FxCop 10.0 (included in the [Windows SDK v7.1][1])

**Note:** FxCop 1.36 is included with Visual Studio 2010. I have not tested this version.

  1. Open your FxCop project, or create a new one.
  2. Go to *Project > Add Rules*
  3. Find RxCop.dll
  4. Choose the 'Rules' tab and verify 'Reactive Extensions Rules' is present
  5. Customize the active rules.

To build the source, you will need C# and F# 4.0 in addition to FxCop 10.0. The rules are 
written in F# and the test cases are written in C#.

### Helpful Resources

I have found [Jason Kresowaty's documentation][2] to be an excelent guide to working with
FxCop, which is otherwise undocumetned. He also has a useful 'Introspector' tool, which I
have included in source root. Thanks, Jason!

Since RX is primarily based on method calls, I have provided processMethodCalls in the Ops module,
which, given a member, will find all its method calls and the verify them with your provided
processor.

## Current Rules

### Usage

  * RX1001: Do not use a blocking call on an observable sequence.
  * RX1002: Provide a scheduler explicitly on calls that create concurrency when possible.

### Design

  * Rx2001: Do not implement reactive framework interfaces unless you are creating a generic construct.

## To-Do

  * Use ObserveOn only after all concurrent calls.
  * Provide error handling for all externally sourced observable sequences.
  * Avoid mid-object lifetime disposal of sequences.
  * Avoid unlimited buffers.
  * Put all side-effects in Do statements.
  * Use Publish to share side effects.
  * Protect calls to user code from within an operator.
  * Subscribe implementations should not throw.
  * Serialize calls to IObserver methdos within observable sequence implementations.
  * Provide an input for an IScheduler in concurrent operations.
  * The scheduler should be the last operator.

  [1]: http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=8279
  [2]: http://www.binarycoder.net/fxcop/html/
