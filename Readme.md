1. Run with Release
2. See disasm after JIT for every public method.
3. I'm not sure if it's an issue since I know nothing with JIT, but has some questions.
    1. Why all `ThrowWithCall` has `rep stos` but not for `ThrowDirect`, the latter one is about 50% faster in all cases;
    2. Why when call directly into `CheckIndex0ThrowWithCall`, the Branch Prediction didn't work well by move throw to the end but it's well for all others;
    3. What's the different between `CheckIndexThrowDirect` and `CheckIndexThrowDirect2`, why jmp `Test` in the first case and call `Test` for the second.