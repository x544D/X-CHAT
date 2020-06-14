CREATE DATABASE IbtiDb
go
use IbtiDb
go


Create Table Themes(id int identity primary key, themeName nvarchar(60))

Create Table Users(id int identity primary key , fullName nvarchar(60), age int, username nvarchar(60), password nvarchar(60),
	totalmsgs int, phone nvarchar(60), email nvarchar(60), theme int foreign key references Themes(id), isBan int, isLogged int, isAdmin int
)


--INSERT into Themes values('Dark')
INSERT into Users VALUES('Directeur', 25 , 'saad','1234',0,'0612899034', 'saad@live.fr', 1, 0, 0, 1) --admin user


--declare @i int = 1
--declare @TmpName nvarchar(60)

--while @i < 100
--	begin
--		PRINT @TmpName
--		SET @TmpName = 'saad'+CONVERT(nvarchar(2) , @i)
--		INSERT into Users VALUES('Amrani '+@TmpName , 15, @TmpName ,'1234',0, '0612899034', @TmpName+'@live.fr', 1, 0, 0 , 0)
--		SET @i = @i  + 1
--	end


INSERT into Users VALUES('ibtihal' , 15,'ibtihal' ,'1234',0, '0612899034','ibti@live.fr', 1, 0, 0 , 0)
INSERT into Users VALUES('rachid' , 15,'rachid' ,'1234',0, '0612899034','ibti@live.fr', 1, 0, 0 , 0)
INSERT into Users VALUES('ayoub' , 15,'ayoub' ,'1234',0, '0612899034','ibti@live.fr', 1, 0, 0 , 0)
INSERT into Users VALUES('mouhsin' , 15,'mouhsin' ,'1234',0, '0612899034','ibti@live.fr', 1, 0, 0 , 0)
INSERT into Users VALUES('soufian' , 15,'soufian' ,'1234',0, '0612899034','ibti@live.fr', 1, 0, 0 , 0)
INSERT into Users VALUES('Holako' , 15,'fati' ,'1234',0, '0612899034','ibti@live.fr', 1, 0, 0 , 0)

CREATE TABLE MMessages(id int identity primary key, content Text , _from int not null foreign key references Users(id),
	_to int default NULL foreign key references Users(id) , mTime datetime , isFiltred int default 0
 )
 

create table _SERVER(up bit default 1) 
insert into _SERVER Values(1)






