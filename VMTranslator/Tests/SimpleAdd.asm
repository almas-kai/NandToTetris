// <- BEGIN CONSTANT PUSH/POP -> 
@7
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
@8
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

// <- BEGIN END PROGRAM -> 
(END)
@END
0;JMP
// <- END END PROGRAM -> 

