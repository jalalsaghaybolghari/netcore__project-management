SET IDENTITY_INSERT ProjectRoles ON
INSERT INTO ProjectRoles (Id,Title)
	VALUES (1,N'manager'),
		   (2,N'member');
SET IDENTITY_INSERT ProjectRoles OFF

SET IDENTITY_INSERT RolePermissions ON
INSERT INTO RolePermissions (Id,Action,ProjectRoleId)
	VALUES 
	(1,N'AddMemmberShip',1),
	(2,N'CreateTask',1),
	(3,N'CreateTask',2),
	(4,N'AssignTask',1),
	(5,N'AssignTask',2),
	(6,N'ChangeTaskStatuses',1),
	(7,N'ChangeTaskStatuses',2),
	(8,N'GetProjectMembers',1),
	(9,N'GetProjectMembers',2)
	;
SET IDENTITY_INSERT RolePermissions OFF

