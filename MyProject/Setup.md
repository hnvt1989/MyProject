//NOTE: DO NOT execute the commented lines

// Enable-Migrations -ProjectName MyProject -ContextTypeName MyProject.DAL.ShoppingCartContext
// Enable-Migrations -ProjectName MyProject -ContextTypeName MyProject.DAL.IdentityContext

// Update-Database -ProjectName MyProject -ConfigurationTypeName MyProject.DAL.DatabaseMasterContext
Update-Database -ProjectName MyProject -ConfigurationTypeName Configuration_IdentityDb
Update-Database -ProjectName MyProject -ConfigurationTypeName Configuration_ShoppingCart

//If run into the EF Migration object already exists error, run the 2 lines below
//Add-Migration InitialCreate –IgnoreChanges -ProjectName MyProject -ConfigurationTypeName Configuration_ShoppingCart
//Update-Database -ProjectName MyProject -ConfigurationTypeName Configuration_ShoppingCart

//create initial db values that were not taken care of by migration
INSERT INTO dbo.Culture (Code, [Description], [Default]) values ('en-US', 'US English', 1);

insert into dbo.AppSetting (Code, [Description], [Value], [ValueType])
  values('SelectedCulture', 'Selected Culture', 'en-US', 'US English')

insert into dbo.ContentType (Code, [Description]) values ('Ad', 'Advertisement')

update dbo.Product set Active = 1

//Manualy insert into db
dbo.AspNetRoles with values (1, 'Admin'), (2, 'Consultant')

//Manualy insert into db
dbo.OrderStatus with values (1, 'BackOrder', 'Back Order' ), (2, 'Processing', 'Processing')

//Map created users with either Admin or consultant roles for administrative access (menus)