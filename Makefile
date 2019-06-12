##
## Makefile
## 
## Made by Vice Phenek
##

RM     =       rm -rf

CSRC   =       */bin */obj

PROJ   = Global.InputForms.sln

BUILD  = msbuild 

NUSPEC = *.nuspec

PACK   = nuget pack

RELEASE = /p:Configuration=Release

build:
		$(BUILD) $(PROJ) $(RELEASE)

nuget:
		$(PACK) $(NUSPEC)

clean:
		$(RM) $(CSRC)

.PHONY:         clean fclean
 
