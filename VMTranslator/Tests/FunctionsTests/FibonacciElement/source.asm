// <- BEGIN BOOTSTRAPPING CODE ->
@256
D=A
@SP
M=D
// <- BEGIN CALL ->
@returnSys.init0
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@LCL
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@ARG
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@THIS
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@THAT
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@SP
D=M
@5
D=D-A
@ARG
M=D
@SP
D=M
@LCL
M=D
@Sys.init
0;JMP
(returnSys.init0)
// <- END CALL ->
// <- END BOOTSTRAPPING CODE ->

// <- BEGIN FUNCTION ->
(Main.fibonacci)
// <- END FUNCTION ->

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

// <- BEGIN LESS THAN -> 
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
D=D-M
// <- END POP -> 
@LESS_1
D;JLE
D=-1
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@END_LESS_1
0;JMP
(LESS_1)
D=0
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
(END_LESS_1)
// <- END LESS THAN -> 

// <- BEGIN GOTO ->
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@N_LT_2
D;JNE
// <- END GOTO ->

// <- BEGIN GOTO ->
@N_GE_2
0;JMP
// <- END GOTO ->

// <- BEGIN LABEL ->
(N_LT_2)
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

// <- BEGIN RETURN ->
@LCL
D=M
@R13
M=D
@5
D=A
@R13
A=M-D
D=M
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

// <- BEGIN LABEL ->
(N_GE_2)
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

// <- BEGIN CALL ->
@returnMain.fibonacci2
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@LCL
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@ARG
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@THIS
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@THAT
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@SP
D=M
@6
D=D-A
@ARG
M=D
@SP
D=M
@LCL
M=D
@Main.fibonacci
0;JMP
(returnMain.fibonacci2)
// <- END CALL ->

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

// <- BEGIN CALL ->
@returnMain.fibonacci3
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@LCL
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@ARG
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@THIS
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@THAT
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@SP
D=M
@6
D=D-A
@ARG
M=D
@SP
D=M
@LCL
M=D
@Main.fibonacci
0;JMP
(returnMain.fibonacci3)
// <- END CALL ->

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

// <- BEGIN RETURN ->
@LCL
D=M
@R13
M=D
@5
D=A
@R13
A=M-D
D=M
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
(END_4)
@END_4
0;JMP
// <- END END PROGRAM -> 

// <- BEGIN FUNCTION ->
(Sys.init)
// <- END FUNCTION ->

// <- BEGIN CONSTANT PUSH/POP -> 
@4
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

// <- BEGIN CALL ->
@returnMain.fibonacci5
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@LCL
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@ARG
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@THIS
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@THAT
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
@SP
D=M
@6
D=D-A
@ARG
M=D
@SP
D=M
@LCL
M=D
@Main.fibonacci
0;JMP
(returnMain.fibonacci5)
// <- END CALL ->

// <- BEGIN LABEL ->
(END)
// <- END LABEL ->

// <- BEGIN GOTO ->
@END
0;JMP
// <- END GOTO ->

// <- BEGIN END PROGRAM -> 
(END_6)
@END_6
0;JMP
// <- END END PROGRAM -> 

