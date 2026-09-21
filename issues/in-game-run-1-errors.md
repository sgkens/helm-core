Here's the transcription. The two screenshots are separate scroll positions, so any errors between `Program(65,64)` and `Program(333,304)` weren't captured. The first line of the second image is clipped at the top, so I've marked those digits as uncertain.

```
Program(27,44): Error: } expected
Program(1,0): Error: A using clause must precede all other elements defined in the namespace except extern alias declarations
Program(1,13): Error: A using clause must precede all other elements defined in the namespace except extern alias declarations
Program(1,46): Error: A using clause must precede all other elements defined in the namespace except extern alias declarations
Program(1,64): Error: A using clause must precede all other elements defined in the namespace except extern alias declarations
Program(1,92): Error: A using clause must precede all other elements defined in the namespace except extern alias declarations
Program(1,123): Error: A using clause must precede all other elements defined in the namespace except extern alias declarations
Program(65,64): Error: Feature 'pattern matching' is not available in C# 6. Please use language version 7.0 or [cut off]

[... errors between these two screenshots not captured ...]

Program(333,304): Error: 'MySprite' does not contain a definition for 'Rotation' and no accessible extension method 'Rotation' accepting a first argument of type 'MySprite' could be found (are you missing a using directive or an assembly reference?)
Program(341,68): Error: The type or member 'Tuple<string, string>' is prohibited
Program(343,167): Error: The type or member 'Tuple' is prohibited
Program(343,173): Error: The type or member 'Tuple<string, string> Tuple.Create<string, string>(string item1, string item2)' is prohibited
Program(361,206): Error: The type or member 'Tuple' is prohibited
Program(361,212): Error: The type or member 'Tuple<string, string> Tuple.Create<string, string>(string item1, string item2)' is prohibited
Program(362,246): Error: The type or member 'string Tuple<string, string>.Item1' is prohibited
Program(367,537): Error: The type or member 'string Tuple<string, string>.Item2' is prohibited
```

context:

- The game's Programmable Block compiles as **C# 6**, so no pattern matching (`is Type x`, `switch` patterns), no `out var`, and no tuples.
- `Tuple` is on the game's prohibited list. Replace it with a small custom struct/class or `KeyValuePair<string, string>`.
- The `using` errors at line 1 are probably a cascade from the `} expected` error at (27,44). They may also mean `using` lines are inside the `Program` class, or that the file has multiple `using` statements on one line (the columns 0, 13, 46, 64, 92, 123 suggest exactly that).
- `MySprite` has no `Rotation` member. It's a struct with a `RotationOrScale` field instead.
