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
(Class2.set)
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

// <- BEGIN STATIC PUSH/POP -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@Class2.0
M=D
// <- END STATIC PUSH/POP -> 

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

// <- BEGIN STATIC PUSH/POP -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@Class2.1
M=D
// <- END STATIC PUSH/POP -> 

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

// <- BEGIN FUNCTION ->
(Class2.get)
// <- END FUNCTION ->

// <- BEGIN STATIC PUSH/POP -> 
@Class2.0
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END STATIC PUSH/POP -> 

// <- BEGIN STATIC PUSH/POP -> 
@Class2.1
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END STATIC PUSH/POP -> 

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
(END_1)
@END_1
0;JMP
// <- END END PROGRAM -> 

// <- BEGIN FUNCTION ->
(Sys.init)
// <- END FUNCTION ->

// <- BEGIN CONSTANT PUSH/POP -> 
@6
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

// <- BEGIN CALL ->
@returnClass1.set2
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
@7
D=D-A
@ARG
M=D
@SP
D=M
@LCL
M=D
@Class1.set
0;JMP
(returnClass1.set2)
// <- END CALL ->

// <- BEGIN TEMP PUSH/POP -> 
@5
D=A
@0
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

// <- BEGIN CONSTANT PUSH/POP -> 
@23
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
@15
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
@returnClass2.set3
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
@7
D=D-A
@ARG
M=D
@SP
D=M
@LCL
M=D
@Class2.set
0;JMP
(returnClass2.set3)
// <- END CALL ->

// <- BEGIN TEMP PUSH/POP -> 
@5
D=A
@0
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

// <- BEGIN CALL ->
@returnClass1.get4
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
@Class1.get
0;JMP
(returnClass1.get4)
// <- END CALL ->

// <- BEGIN CALL ->
@returnClass2.get5
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
@Class2.get
0;JMP
(returnClass2.get5)
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

// <- BEGIN FUNCTION ->
(Class1.set)
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

// <- BEGIN STATIC PUSH/POP -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@Class1.0
M=D
// <- END STATIC PUSH/POP -> 

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

// <- BEGIN STATIC PUSH/POP -> 
// <- BEGIN POP -> 
@SP
M=M-1
A=M
D=M
// <- END POP -> 
@Class1.1
M=D
// <- END STATIC PUSH/POP -> 

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

// <- BEGIN FUNCTION ->
(Class1.get)
// <- END FUNCTION ->

// <- BEGIN STATIC PUSH/POP -> 
@Class1.0
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END STATIC PUSH/POP -> 

// <- BEGIN STATIC PUSH/POP -> 
@Class1.1
D=M
// <- BEGIN PUSH -> 
@SP
A=M
M=D
@SP
M=M+1
// <- END PUSH -> 
// <- END STATIC PUSH/POP -> 

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
(END_7)
@END_7
0;JMP
// <- END END PROGRAM -> 

