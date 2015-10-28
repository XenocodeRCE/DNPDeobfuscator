# DNPDeobfuscator
Open-source deobfuscator for dotnetpatcher (https://bitbucket.org/3dotdev/dotnet-patcher/src/) written in C#. 
It does support some options (see bellow). Most deobfuscation is done through DNPDeobfuscator but all the numeric 
calculation is done thanks to modded CodeCracker's tool.

# Features

Here's a pseudo list of the things which are currently supported with DNPDeobfuscator :

 - String Encryption
  - String Encryption
  - String Encryption + Resource Encryption
  - String Encryption + Resource Compression
 - Integers Encryption
  - Local decryption
  - Dynamic decryption
  - Resource decryption
 - Resource Encryption
 - Resource Compression
 - Invalid OpCode -> I cheat on this one, nobody use it since it makes your file crashes.
 - Prune useless Methods / Types
 - Call Hiding
  - Call Hiding with Integers
  - Call Hiding with Strings
 
# HowTo

 - Remove Invalid Metadata - > ![De4Dot](https://github.com/0xd4d/de4dot) does the job
 - Clean constant calculation -> Uses ![my constants tool pack](https://github.com/XenocodeRCE/DNPDeobfuscator/releases/tag/v0.0)
  - Use AppFuscatorConstantFill2.exe
  - Then use AppFuscatorOperationCalc.exe
  - Then use de4dot
 - Now you can use DNPDeobfuscator.
