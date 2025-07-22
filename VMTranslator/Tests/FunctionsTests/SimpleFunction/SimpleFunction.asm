// <- BEGIN FUNCTION ->
(SimpleFunction.function)
@0
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@0
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END FUNCTION ->

// <- BEGIN LCL PUSH/POP -> 
@0
D=A
@LCL
D=D+M
@R13
M=D
@R13
A=M
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END LCL PUSH/POP -> 

// <- BEGIN LCL PUSH/POP -> 
@1
D=A
@LCL
D=D+M
@R13
M=D
@R13
A=M
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END LCL PUSH/POP -> 

// <- BEGIN ADD -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=D+M
// <- END POP -> 
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END ADD -> 

// <- BEGIN NOT -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=!M
// <- END POP -> 
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END NOT -> 

// <- BEGIN ARG PUSH/POP -> 
@0
D=A
@ARG
D=D+M
@R13
M=D
@R13
A=M
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END ARG PUSH/POP -> 

// <- BEGIN ADD -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=D+M
// <- END POP -> 
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END ADD -> 

// <- BEGIN ARG PUSH/POP -> 
@1
D=A
@ARG
D=D+M
@R13
M=D
@R13
A=M
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END ARG PUSH/POP -> 

// <- BEGIN SUBTRACT -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M-D
// <- END POP -> 
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END SUBTRACT -> 

// <- BEGIN RETURN ->
@LCL
D=M
@R13
M=D
@5
D=A
@R13
D=M-D
@R14
M=D
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@ARG
A=M
M=D
@ARG
D=M
@1
D=D+A
@SP
M=D
@R13
AM=M-1
D=M
@THAT
M=D
@R13
AM=M-1
D=M
@THIS
M=D
@R13
AM=M-1
D=M
@ARG
M=D
@R13
AM=M-1
D=M
@LCL
M=D
@R14
A=M
0;JMP
// <- END RETURN ->

// <- BEGIN END PROGRAM -> 
(END_0)
@END_0
0;JMP
// <- END END PROGRAM -> 

