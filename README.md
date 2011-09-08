# RxCop

RxCop is a rule library that works with FxCop to provide additional checking 
for your observable sequences.

## Getting Started

To use RxCop, you will need to have installed FxCop 10.0 (included in the
[Windows SDK v7.1][1] and Visual Studio 2010 Premium and Ultimate)

### Visual Studio Installation

  1. Build or obtain RxCop.dll from the downloads.
  2. Copy RxCop.dll to "${env:ProgramFiles(x86)}\Microsoft Visual Studio 10.0\Team Tools\Static Analysis Tools\FxCop\Rules"
  3. Navigate to the desired project's property page
  4. Choose the *Code Analysis* tab.
  5. Click the *Open* button
  6. If this is not a custom ruleset, choose *File > Save As...* and create a new file.
  7. Choose *Group by: Category*
  8. If you cannot find NorthHorizon.Design or NorthHorizon.Usage, right click in the grid area and choose *Show rules that are not enabled*

At least you don't have to modify the registry.

### FxCop Standalone Installation

  1. Build or obtain RxCop.dll from the downloads.
  2. Open your FxCop project, or create a new one.
  3. Go to *Project > Add Rules*
  4. Find RxCop.dll
  5. Choose the 'Rules' tab and verify 'Reactive Extensions Rules' is present
  6. Customize the active rules.

## Hacking RxCop

To build the source, you will need C# and F# 4.0 in addition to FxCop 10.0. The
rules are written in F# and the test cases are written in C#.

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
