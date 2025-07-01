// <- BEGIN CONSTANT PUSH/POP -> 
@10
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

// <- BEGIN CONSTANT PUSH/POP -> 
@21
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

// <- BEGIN CONSTANT PUSH/POP -> 
@22
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

// <- BEGIN ARG PUSH/POP -> 
@2
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
@1
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

// <- BEGIN CONSTANT PUSH/POP -> 
@36
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

// <- BEGIN THIS PUSH/POP -> 
@6
D=A
@THIS
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
// <- END THIS PUSH/POP -> 

// <- BEGIN CONSTANT PUSH/POP -> 
@42
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

// <- BEGIN CONSTANT PUSH/POP -> 
@45
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
@5
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

// <- BEGIN CONSTANT PUSH/POP -> 
@510
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

// <- BEGIN TEMP PUSH/POP -> 
@5
D=A
@6
D=D+A
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
// <- END TEMP PUSH/POP -> 

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

// <- BEGIN THAT PUSH/POP -> 
@5
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

// <- BEGIN THIS PUSH/POP -> 
@6
D=A
@THIS
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
// <- END THIS PUSH/POP -> 

// <- BEGIN THIS PUSH/POP -> 
@6
D=A
@THIS
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
// <- END THIS PUSH/POP -> 

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

// <- BEGIN TEMP PUSH/POP -> 
@5
D=A
@6
D=D+A
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
// <- END TEMP PUSH/POP -> 

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

// <- BEGIN END PROGRAM -> 
(END)
@END
0;JMP
// <- END END PROGRAM -> 

