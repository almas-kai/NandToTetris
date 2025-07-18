// <- BEGIN CONSTANT PUSH/POP -> 
@0
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

// <- BEGIN LCL PUSH/POP -> 
@0
D=A
@LCL
D=D+M
@R13
M=D
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@R13
A=M
M=D
// <- END LCL PUSH/POP -> 

// <- BEGIN LABEL ->
(LOOP)
// <- END LABEL ->

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

// <- BEGIN LCL PUSH/POP -> 
@0
D=A
@LCL
D=D+M
@R13
M=D
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@R13
A=M
M=D
// <- END LCL PUSH/POP -> 

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

// <- BEGIN CONSTANT PUSH/POP -> 
@1
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

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

// <- BEGIN ARG PUSH/POP -> 
@0
D=A
@ARG
D=D+M
@R13
M=D
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@R13
A=M
M=D
// <- END ARG PUSH/POP -> 

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

// <- BEGIN GOTO ->
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@LOOP
D;JNE
// <- END GOTO ->

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

// <- BEGIN END PROGRAM -> 
(END_0)
@END_0
0;JMP
// <- END END PROGRAM -> 

