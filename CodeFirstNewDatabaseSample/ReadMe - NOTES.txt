1. Make a C# application using Entity Framework Code First.

NOTE: 	For this Project I am follwing a tutorial from this website:
	https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database

NOTE:	There are minor differences that I had to make since the tutorial is a bit outdated (They're using Visual Studio 2012).

NOTE: 	In Visual Studio 2019, first I went to 'File', 'New Project...', then I selected 'Console App (.NET Framework)'
 	and called it, 'CodeFirstNewDatabaseSample'.

NOTE:	The first thing to do was install EntityFramework by going to 'Tools', 'Nuget Package Manager', Then
	'Manage NuGet Packages for Solution...'. There I was able to install EntityFramework into my program.

NOTE: 	Now I started to code in my 'Program.cs' file, creating classes that will essentally map to a database 
	(which is the whole point of the code first approach).
--------------------------------------------
CodeFirstNewDatabaseSample/Program.cs
--------------------------------------------
using System;
using System.Data.Entity; //NOTE: This is needed to use base class 'DbContext' (Entity Framwork is needed to use 'System.Data.Entity').
using System.Linq; //NOTE: This is needed for IOrderedQueryable<Blog> type.
using System.Collections.Generic;

namespace CodeFirstNewDatabaseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //NOTE: Here we are creating a new BlogContext database, 'db'.
            using (BlogContext db = new BlogContext())
            {
                Console.Write("Enter a namefor a new blog:");
                string name = Console.ReadLine();

                //NOTE: Here we are creating a new 'Blog' object, and assigning 'name' to the object's 'Name' property. 
                Blog blog = new Blog { Name = name };

                //NOTE: Object db has a property 'Blogs' which is a database set of 'Blog' objects ('DbSet<Blog>'), and 
                //      we are adding our currnet 'blog' object to that set.
                db.Blogs.Add(blog);

                //NOTE: Here we are saving changes to the database.
                db.SaveChanges();

                //NOTE: 'IOrderedQueryable<Blog>' type represents the result of a sorting operation.
                IOrderedQueryable<Blog> query = from b in db.Blogs
                                                orderby b.Name
                                                select b;

                foreach (Blog item in query)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }

        //NOTE: I'm adding the 'virtual' keyword into the navagation properties, this enables the entity
        //      framework feature called, 'Lazy Loading'. This feature means the Entity Framwork will 
        //      automatically query and populate the content of these properties whenever I try to 
        //      access them.
        public virtual Blog Blog { get; set; }
    }

    //NOTE: After adding Entity Framwork to our project, we can go ahead and define a derived context,
    //      BlogContext, which derives from 'DbContext' class (from the Entity Framework).

    public class BlogContext : DbContext
    {
        //NOTE: This DbSet, allows us to query inside instances of those types ('Blog' and 'Post').
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }

}
--------------------------------------------

NOTE: 	After coding that, I ran it and entered, entered 'ASP.NET Blog' pressed enter,
	then it printed the blog that was created back to us, which is coming out of that
	code (the foreach loop) that we created wich iterated through the query list,
	'IOrderedQueryable<Blog> query' that we created.
NOTE: 	This is the Console window.
--------------------------------------------
Enter a namefor a new blog:ASP.NET Blog
ASP.NET Blog
--------------------------------------------

NOTE: 	After doing that when I go check my databases by going to 'SQL Server Object Explorer' 
	After I click on my databse (the name of mine is DESKTOP-MEVIRUK\SQLEXPRESS),
	under the 'Databases' folder, I can see 'CodeFirstNewDatabaseDample.BlogContext' database was
	automatically created for me. Ans when I see go into the 'Tables' folder, I can see
	'dbo.Blogs' and 'dbo.Posts' and the columns match the class properties we created. 

NOTE:	Now as our application evolves over time we gonna need to make changes to the 
	domain model (our classes), and that in return would mean we need to make changes 
	to the database schema.
NOTE:	To do this we're gonna make use of a feature called, 'Code First Migrations'. 

NOTE: 	To start using 'Code First Migrations', we go to 'Tools', 'Nuget Package Manager', Then
	'Package Manager Console'. Then we are going to rin the 'Enable-Migrations' command 
	shown below.
--------------------------------------------
Package Manager Console
--------------------------------------------
PM> Enable-Migrations
Checking if the context targets an existing database...
Detected database created with a database initializer. Scaffolded migration '201909030649210_InitialCreate' corresponding to existing database. 
To use an automatic migration instead, delete the Migrations folder and re-run Enable-Migrations specifying the -EnableAutomaticMigrations parameter.
Code First Migrations enabled for project CodeFirstNewDatabaseSample.
PM> 
--------------------------------------------
NOTEL: 	This is gonna look in my project, find the BlogContext and go ahead and enable Code First Migrations for the
	Blog Context. Doing this has added two things to my project, under the 'Migrations Folder':
	'201909030649210_InitialCreate.cs' and 'Configuration.cs'.
NOTE:	The Configuration class contains all of  the configurations and settings for migrations for our 
	BlogContext. We don't need to change anything here at the moment, but this is a place where you can
	change the folders the migrations are added to, register providers for third party databases, or 
	or specify 'seed' data.
--------------------------------------------
CodeFirstNewDatabaseSample/Migrations/Configuration.cs
--------------------------------------------
namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeFirstNewDatabaseSample.BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CodeFirstNewDatabaseSample.BlogContext";
        }

        protected override void Seed(CodeFirstNewDatabaseSample.BlogContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
--------------------------------------------

NOTE: 	The other thing that was added is an 'InitialCreate' migration. It added this beacuse we already created
	the database and applied the set of changes. Remember we already created the blog in 'Post' table.
	If we look inside this migration we'll see some code that represents creating that blog in that 'Post'
	table in the database. Code First migrations is also recorded in my local database, indicating that this 
	migration has already been applied.

--------------------------------------------
CodeFirstNewDatabaseSample/Migrations/201909030649210_InitialCreate.cs
--------------------------------------------
namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        BlogId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BlogId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        BlogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.BlogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "BlogId", "dbo.Blogs");
            DropIndex("dbo.Posts", new[] { "BlogId" });
            DropTable("dbo.Posts");
            DropTable("dbo.Blogs");
        }
    }
}
--------------------------------------------

NOTE:	Now that we've enabled migrations, lets go ahead  and make a change to our domain model (our classes).
--------------------------------------------
CodeFirstNewDatabaseSample/Program.cs
--------------------------------------------
using System;
using System.Data.Entity; //NOTE: This is needed to use base class 'DbContext' (Entity Framwork is needed to use 'System.Data.Entity').
using System.Linq; //NOTE: This is needed for IOrderedQueryable<Blog> type.
using System.Collections.Generic;

namespace CodeFirstNewDatabaseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BlogContext db = new BlogContext())
            {
                Console.Write("Enter a namefor a new blog:");
                string name = Console.ReadLine();

                Blog blog = new Blog { Name = name };

                db.Blogs.Add(blog);
.
                db.SaveChanges();

                IOrderedQueryable<Blog> query = from b in db.Blogs
                                                orderby b.Name
                                                select b;

                foreach (Blog item in query)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

	//NOTE: Now we added this property.
        public string Url { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }

}
--------------------------------------------

NOTE: 	After adding the 'Url' property to the 'Blog' class, we will run a 'Add-Migration AddUrl' command into the 
	'Package Manager Console' to scaffle those changes. As you can see in the command, Add-Migration allows 
	you to name that migration (AddUrl).

--------------------------------------------
Package Manager Console
--------------------------------------------
PM> Add-Migration AddUrl
Scaffolding migration 'AddUrl'.
The Designer Code for this migration file includes a snapshot of your current Code First model. This snapshot is used to calculate the changes to your model when you scaffold the next migration. If you make additional changes to your model that you want to include in this migration, then you can re-scaffold it by running 'Add-Migration AddUrl' again.
PM> 
--------------------------------------------

NOTE: 	Once this is done, a new migration ('201909030830491_AddUrl.cs') appears in the migrations folder.
	If we open this new migration, we see some simple code that has an upgrade and a downgrade method that
	says to add the url column on the way up, and to drop it on the way down.
--------------------------------------------
CodeFirstNewDatabaseSample/Migrations/201909030830491_AddUrl.cs
--------------------------------------------
namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "Url");
        }
    }
}
--------------------------------------------

NOTE:	Now that we have a new migration I can use the 'Update-Database' command to go ahead and apply 
	any pending changes to the database.
--------------------------------------------
Package Manager Console
--------------------------------------------
PM> Update-Database
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Applying explicit migrations: [201909030830491_AddUrl].
Applying explicit migration: 201909030830491_AddUrl.
Running Seed method.
PM> 
--------------------------------------------
NOTE:	When I run this command is gonna look in the project and see my two migrations, its gonna look in 
	the database see that the initial created migration has already been applied, which means it just needs
	to go ahead and apply the Url migration.

NOTE:	With this done if I go back to the database, and I refresh the 'Blogs' table, ill the the 'Url' column 
	is now present.
NOTE:	So far we've just relied on 'Code First' conventions to interpret the shape of our classes, and to decide
	what the database will look like. But there are gonna be times where your classes aren't gonna
	follow what 'Code First' is expecting, thus you may want to change what the database looks like.
NOTE:	There are two options for doing this: 'Data Annotations' & A 'fluid API'.

NOTE:	Lets take a look at 'Data Annotations'.

NOTE: 	Now I am Gonna add a 'User' class with a 'UserName' property and a 'DisplayName' property.
	I am also going to add a 'Users' property in our BlogContext class so that the users are
	included on that model.
--------------------------------------------
CodeFirstNewDatabaseSample/Program.cs
--------------------------------------------
using System;
using System.Data.Entity; //NOTE: This is needed to use base class 'DbContext' (Entity Framwork is needed to use 'System.Data.Entity').
using System.Linq; //NOTE: This is needed for IOrderedQueryable<Blog> type.
using System.Collections.Generic;

namespace CodeFirstNewDatabaseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //NOTE: Here we are creating a new BlogContext database, 'db'.
            using (BlogContext db = new BlogContext())
            {
                Console.Write("Enter a namefor a new blog:");
                string name = Console.ReadLine();

                //NOTE: Here we are creating a new 'Blog' object, and assigning 'name' to the object's 'Name' property. 
                Blog blog = new Blog { Name = name };

                //NOTE: Object db has a property 'Blogs' which is a database set of 'Blog' objects ('DbSet<Blog>'), and 
                //      we are adding our currnet 'blog' object to that set.
                db.Blogs.Add(blog);

                //NOTE: Here we are saving changes to the database.
                db.SaveChanges();

                //NOTE: 'IOrderedQueryable<Blog>' type represents the result of a sorting operation.
                IOrderedQueryable<Blog> query = from b in db.Blogs
                                                orderby b.Name
                                                select b;

                foreach (Blog item in query)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }

        //NOTE: I'm adding the 'virtual' keyword into the navagation properties, this enables the entity
        //      framework feature called, 'Lazy Loading'. This feature means the Entity Framwork will 
        //      automatically query and populate the content of these properties whenever I try to 
        //      access them.
        public virtual Blog Blog { get; set; }
    }

    //NOTE: After adding Entity Framwork to our project, we can go ahead and define a derived context,
    //      BlogContext, which derives from 'DbContext' class (from the Entity Framework).

    public class BlogContext : DbContext
    {
        //NOTE: This DbSet, allows us to query inside instances of those types ('Blog' and 'Post').
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
--------------------------------------------

NOTE: 	With this done if I go ahead and run a 'Add-Migration AddUser' you'll see i'll get an excpetion
	because Code First can't work out which property should be the primary key for your user.
--------------------------------------------
Package Manager Console
--------------------------------------------
PM> Add-Migration AddUser
System.Data.Entity.ModelConfiguration.ModelValidationException: One or more validation errors were detected during model generation:

CodeFirstNewDatabaseSample.User: : EntityType 'User' has no key defined. Define the key for this EntityType.
Users: EntityType: EntitySet 'Users' is based on type 'User' that has no keys defined.

   at System.Data.Entity.Core.Metadata.Edm.EdmModel.Validate()
   at System.Data.Entity.DbModelBuilder.Build(DbProviderManifest providerManifest, DbProviderInfo providerInfo)
   at System.Data.Entity.DbModelBuilder.Build(DbConnection providerConnection)
   at System.Data.Entity.Internal.LazyInternalContext.CreateModel(LazyInternalContext internalContext)
   at System.Data.Entity.Internal.RetryLazy`2.GetValue(TInput input)
   at System.Data.Entity.Internal.LazyInternalContext.InitializeContext()
   at System.Data.Entity.Internal.LazyInternalContext.get_ModelBeingInitialized()
   at System.Data.Entity.Infrastructure.EdmxWriter.WriteEdmx(DbContext context, XmlWriter writer)
   at System.Data.Entity.Utilities.DbContextExtensions.<>c__DisplayClass1.<GetModel>b__0(XmlWriter w)
   at System.Data.Entity.Utilities.DbContextExtensions.GetModel(Action`1 writeXml)
   at System.Data.Entity.Utilities.DbContextExtensions.GetModel(DbContext context)
   at System.Data.Entity.Migrations.DbMigrator..ctor(DbMigrationsConfiguration configuration, DbContext usersContext, DatabaseExistenceState existenceState, Boolean calledByCreateDatabase)
   at System.Data.Entity.Migrations.DbMigrator..ctor(DbMigrationsConfiguration configuration)
   at System.Data.Entity.Migrations.Design.MigrationScaffolder..ctor(DbMigrationsConfiguration migrationsConfiguration)
   at System.Data.Entity.Migrations.Design.ToolingFacade.ScaffoldRunner.RunCore()
   at System.Data.Entity.Migrations.Design.ToolingFacade.BaseRunner.Run()
One or more validation errors were detected during model generation:

CodeFirstNewDatabaseSample.User: : EntityType 'User' has no key defined. Define the key for this EntityType.
Users: EntityType: EntitySet 'Users' is based on type 'User' that has no keys defined.

PM> 
--------------------------------------------

NOTE: 	We can rectify this (fix this) by using 'DataAnnotetions'. We can use this '[Key]' annotation to 
	the 'UserName' property.
--------------------------------------------
CodeFirstNewDatabaseSample/Program.cs
--------------------------------------------
using System;
using System.Data.Entity; //NOTE: This is needed to use base class 'DbContext' (Entity Framwork is needed to use 'System.Data.Entity').
using System.Linq; //NOTE: This is needed for IOrderedQueryable<Blog> type.
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //NOTE: It is needed to uniquely identify and entity in our class model. ([Key] inside of the 'User' class)

namespace CodeFirstNewDatabaseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //NOTE: Here we are creating a new BlogContext database, 'db'.
            using (BlogContext db = new BlogContext())
            {
                Console.Write("Enter a namefor a new blog:");
                string name = Console.ReadLine();

                //NOTE: Here we are creating a new 'Blog' object, and assigning 'name' to the object's 'Name' property. 
                Blog blog = new Blog { Name = name };

                //NOTE: Object db has a property 'Blogs' which is a database set of 'Blog' objects ('DbSet<Blog>'), and 
                //      we are adding our currnet 'blog' object to that set.
                db.Blogs.Add(blog);

                //NOTE: Here we are saving changes to the database.
                db.SaveChanges();

                //NOTE: 'IOrderedQueryable<Blog>' type represents the result of a sorting operation.
                IOrderedQueryable<Blog> query = from b in db.Blogs
                                                orderby b.Name
                                                select b;

                foreach (Blog item in query)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class User
    {
        [Key] //NOTE: Here is the Key annotion.
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }

        //NOTE: I'm adding the 'virtual' keyword into the navagation properties, this enables the entity
        //      framework feature called, 'Lazy Loading'. This feature means the Entity Framwork will 
        //      automatically query and populate the content of these properties whenever I try to 
        //      access them.
        public virtual Blog Blog { get; set; }
    }

    //NOTE: After adding Entity Framwork to our project, we can go ahead and define a derived context,
    //      BlogContext, which derives from 'DbContext' class (from the Entity Framework).

    public class BlogContext : DbContext
    {
        //NOTE: This DbSet, allows us to query inside instances of those types ('Blog' and 'Post').
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
--------------------------------------------

NOTE: 	After doing that, If I run a 'Add-Migraton AddUser' command, it will scaffold the model to 
	the database successfully. . 

-------------------------------------------
Package Manager Console
-------------------------------------------
PM> Add-Migration AddUser
Scaffolding migration 'AddUser'.
The Designer Code for this migration file includes a snapshot of your current Code First model. 
This snapshot is used to calculate the changes to your model when you scaffold the next migration. 
If you make additional changes to your model that you want to include in this migration, then you 
can re-scaffold it by running 'Add-Migration AddUser' again.
PM> 
-------------------------------------------

NOTE: 	Now ee will be able to see that an '201909030917151_AddUser.cs' migrations was added in the 
	'Migrations' folder.
-------------------------------------------
CodeFirstNewDatabaseSample/Migrations/201909030917151_AddUser.cs
-------------------------------------------
namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.UserName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
-------------------------------------------

NOTE: 	If I then go ahead and run a 'Update-Database' command, you'll see that the changes are applied to the 
	database.
-------------------------------------------
Package Manager Console
-------------------------------------------
PM> Update-Database
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Applying explicit migrations: [201909030917151_AddUser].
Applying explicit migration: 201909030917151_AddUser.
Running Seed method.
PM> 
-------------------------------------------

NOTE: 	Now if we refresh the 'Tables' folder in 'CodeFirstDatabseSample.BlogContext' under 
	'SQL Server Explorer', we will see that the 'User' yable has been added with the 
	'UserName' field(column) being the Primary key in that table.

NOTE:	'Data Annotations' are a good simple way to overwrite Code First Conventions. Second option is
	a 'flent API'.
NOTE:	The 'Fluent API' is a bit more advanced and covers everything that can be done with data annotations,
	and a little bit more. To explore the 'Fluent API', lets say that we want to change the column that
	the 'DisplayName' property of 'Users' is met to, to be called 'display_name'.

NOTE:	To use a 'Fluent API', I override the 'OnModelCreating' method inside the BlogContext class 
	(which derives DbContext).
NOTE:	take a look at the changes made in the 'BlogContext' class.
--------------------------------------------
CodeFirstNewDatabaseSample/Program.cs
--------------------------------------------
using System;
using System.Data.Entity; //NOTE: This is needed to use base class 'DbContext' (Entity Framwork is needed to use 'System.Data.Entity').
using System.Linq; //NOTE: This is needed for IOrderedQueryable<Blog> type.
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //NOTE: It is needed to uniquely identify and entity in our class model. ([Key] inside of the 'User' class)

namespace CodeFirstNewDatabaseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //NOTE: Here we are creating a new BlogContext database, 'db'.
            using (BlogContext db = new BlogContext())
            {
                Console.Write("Enter a namefor a new blog:");
                string name = Console.ReadLine();

                //NOTE: Here we are creating a new 'Blog' object, and assigning 'name' to the object's 'Name' property. 
                Blog blog = new Blog { Name = name };

                //NOTE: Object db has a property 'Blogs' which is a database set of 'Blog' objects ('DbSet<Blog>'), and 
                //      we are adding our currnet 'blog' object to that set.
                db.Blogs.Add(blog);

                //NOTE: Here we are saving changes to the database.
                db.SaveChanges();

                //NOTE: 'IOrderedQueryable<Blog>' type represents the result of a sorting operation.
                IOrderedQueryable<Blog> query = from b in db.Blogs
                                                orderby b.Name
                                                select b;

                foreach (Blog item in query)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class User
    {

        [Key] //NOTE: Here we ase using this '[Key]' annotation on the 'UserName' property.
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }

        //NOTE: I'm adding the 'virtual' keyword into the navagation properties, this enables the entity
        //      framework feature called, 'Lazy Loading'. This feature means the Entity Framwork will 
        //      automatically query and populate the content of these properties whenever I try to 
        //      access them.
        public virtual Blog Blog { get; set; }
    }

    //NOTE: After adding Entity Framwork to our project, we can go ahead and define a derived context,
    //      BlogContext, which derives from 'DbContext' class (from the Entity Framework).

    public class BlogContext : DbContext
    {
        //NOTE: This DbSet, allows us to query inside instances of those types ('Blog' and 'Post').
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        //NOTE: Here I am using 'Fluent API' in order to change the column that the 'DisplayName' property 
        //      of 'Users' is met to, to be called 'display_name'. We accomplish this by using the 
        //      'OnModelCreating' method
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //NOTE: First we start with the entity we want to configure, in our case is user.
            modelBuilder.Entity<User>()
                .Property(u => u.DisplayName) //NOTE: Here I want to identify That I want to configure the 'DisplayName' property.
                .HasColumnName("display_name"); 
                //NOTE: There are many things we can change but in this exercise we are changing the Column name on the database.
        }
    }

}
--------------------------------------------

NOTE: 	After making that change, we will go ahead and add a migration using command,
	'Add-Migration ChangeDisplayName'
-------------------------------------------
Package Manager Console
-----------------------------------------
PM> Add-Migration ChangeDisplayName
Scaffolding migration 'ChangeDisplayName'.
The Designer Code for this migration file includes a snapshot of your current Code First model. 
This snapshot is used to calculate the changes to your model when you scaffold the next migration. 
If you make additional changes to your model that you want to include in this migration, then you 
can re-scaffold it by running 'Add-Migration ChangeDisplayName' again.
PM> 
-----------------------------------------


NOTE: 	Now when we look at the migration file generated, '201909030951476_ChangeDisplayName.cs'. You'll
	see here the 'Up' method, renames the 'DisplayName' to 'display_name' and the 'Down' method,
	renames it back the other way.
-----------------------------------------
CodeFirstNewDatabaseSample/Migrations/201909030951476_ChangeDisplayName.cs
-----------------------------------------
namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDisplayName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Users", name: "DisplayName", newName: "display_name");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Users", name: "display_name", newName: "DisplayName");
        }
    }
}
-----------------------------------------

NOTE: 	Now we will run the 'Update-Databse' comment on the 'Package Manager Console' to scaffle the
	changes. After refreshing the 'Column' folder under 'dbo.Users' table, well see that the
	field(column) name has been changed to 'display_name'. 

NOTE: 	So far we seen how to:
	1. Create a Code First Model.
	2. How to have a databse created form that model.
	3. How to change that databse, using code first migrations as our code first model changes over time.
	4. We then so, had to overwrite the conventions code first has by using both 'Data Annotations'
	and 'Afluent API'.