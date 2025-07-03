// <- BEGIN CONSTANT PUSH/POP -> 
@3030
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

// <- POINTER PUSH/POP BEGIN -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@THIS
M=D
// <- POINTER PUSH/POP END -> 

// <- BEGIN CONSTANT PUSH/POP -> 
@3040
D=A
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END CONSTANT PUSH/POP -> 

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
@32
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
@2
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
@46
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
@6
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
@THIS
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- POINTER PUSH/POP END -> 

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

// <- BEGIN THIS PUSH/POP -> 
@2
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

// <- BEGIN THAT PUSH/POP -> 
@6
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

// <- BEGIN END PROGRAM -> 
(END)
@END
0;JMP
// <- END END PROGRAM -> 

