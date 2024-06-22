# AsyncWriter
<img src="https://img.shields.io/badge/FSharp-7.0-purple"> <img src="https://img.shields.io/badge/DotNet-7.0-green"> <img src="https://img.shields.io/badge/License-MIT-blue">


This is a sample project demonstrating asynchronous file writing in F#.

## Demo
In the following examples, the writing task takes 4500 milliseconds.

### Example 1: Start Immediately
startTime = 0 [msec]

<img src="https://github.com/hirotk/AsyncWriter/assets/6882458/b4973c0d-1d03-473a-92c7-e07e030af169" alt="ex1" style="max-width:100%">

&nbsp;


### Example 2-1: Delay Start and Join
(startTime, joinTime) = (2000, 4000) [msec]

<img src="https://github.com/hirotk/AsyncWriter/assets/6882458/2d26c6cf-9255-442c-b4c9-d2cdc4953ffe" alt="ex2_1" style="max-width:100%">

### Example 2-2: Delay Start and No Join
(startTime, joinTime) = (2000, 7000) [msec]

<img src="https://github.com/hirotk/AsyncWriter/assets/6882458/eb90569b-3d6e-45c3-b1f2-7166a8f083e4" alt="ex2_2" style="max-width:100%">

&nbsp;


### Example 3-1: Cancel Async Writing
(startTime, cancelTime) = (0, 2000) [msec]

<img src="https://github.com/hirotk/AsyncWriter/assets/6882458/5f14b1b6-a686-4ac6-9e43-8cb36e452b93" alt="ex3_1" style="max-width:100%">

### Example 3-2: Complete Before Cancel
(startTime, cancelTime = (0, 6000) [msec]

<img src="https://github.com/hirotk/AsyncWriter/assets/6882458/21645743-5756-4050-a5a1-716446eea0c5" alt="ex3_2" style="max-width:100%">

&nbsp;
