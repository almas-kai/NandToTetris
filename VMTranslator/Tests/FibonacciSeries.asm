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

// <- POINTER PUSH/POP BEGIN -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@THAT
M=D
// <- POINTER PUSH/POP END -> 

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

// <- BEGIN THAT PUSH/POP -> 
@0
D=A
@THAT
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
// <- END THAT PUSH/POP -> 

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

// <- BEGIN THAT PUSH/POP -> 
@1
D=A
@THAT
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
// <- END THAT PUSH/POP -> 

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
@2
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

// <- BEGIN GOTO ->
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@COMPUTE_ELEMENT
D;JNE
// <- END GOTO ->

// <- BEGIN GOTO ->
@END
0;JMP
// <- END GOTO ->

// <- BEGIN LABEL ->
(COMPUTE_ELEMENT)
// <- END LABEL ->

// <- BEGIN THAT PUSH/POP -> 
@0
D=A
@THAT
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
// <- END THAT PUSH/POP -> 

// <- BEGIN THAT PUSH/POP -> 
@1
D=A
@THAT
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
// <- END THAT PUSH/POP -> 

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

// <- BEGIN THAT PUSH/POP -> 
@2
D=A
@THAT
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
// <- END THAT PUSH/POP -> 

// <- POINTER PUSH/POP BEGIN -> 
@THAT
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- POINTER PUSH/POP END -> 

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

// <- POINTER PUSH/POP BEGIN -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@THAT
M=D
// <- POINTER PUSH/POP END -> 

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

// <- BEGIN GOTO ->
@LOOP
0;JMP
// <- END GOTO ->

// <- BEGIN LABEL ->
(END)
// <- END LABEL ->

// <- BEGIN END PROGRAM -> 
(END_0)
@END_0
0;JMP
// <- END END PROGRAM -> 

