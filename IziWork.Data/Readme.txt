#Lib Rquire
Install-Package Microsoft.EntityFrameworkCore.SqlServer –Pre
Install-Package Microsoft.EntityFrameworkCore.Tools –Pre
Install-Package Microsoft.EntityFrameworkCore.SqlServer.Design –Pre

# Lệnh gen db Local
Scaffold-DbContext 'data source=WS-1012;initial catalog=IziWorkManagement;persist security info=True;user id=sa;password=Matkhau1;MultipleActiveResultSets=True;encrypt=false' Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -f
# Lệnh update
... -f

-------------- UAT
# Lệnh gen db Local
Scaffold-DbContext 'data source=192.168.1.50;initial catalog=UpgradeApplication;persist security info=True;user id=sa;password=Net$1234;MultipleActiveResultSets=True;encrypt=false' Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -f
# Lệnh update
... -f