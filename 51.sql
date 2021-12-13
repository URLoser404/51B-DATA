select [user].id,[user].account,[user].password,[user].errorCount,[user].status,[PermissionArea].name permissionArea,[PermissionUser].name permissionUser from [User]
inner join [PermissionArea] on [user].permissionArea_id = [PermissionArea].id
inner join [PermissionUser] on [User].permissionUser_id = [PermissionUser].id
where [user].account = 'test'
and [user].password = 'test'


update [user] set errorCount=0 where id =1


insert into [user] values(1,1,'test_SGA2','test_SGA2',0,1)

insert into [SystemLog] values(1,1,GETDATE())
 

select time,[user].account,Action.name from SystemLog 
inner join Action 
on SystemLog.action_id = Action.id
inner join [user]
on SystemLog.user_id = [user].id

select GETDATE()



update [user] 
set password = 'test',
errorCount = 0,
status = 1,
permissionArea_id = 1,
permissionUser_id = 7
where id = 1


insert into [User]
values()

