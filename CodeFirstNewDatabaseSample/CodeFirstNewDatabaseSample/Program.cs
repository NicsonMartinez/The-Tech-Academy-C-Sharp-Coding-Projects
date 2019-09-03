using System;
using System.Data.Entity; //NOTE: This is needed to use base class 'DbContext' (Entity Framwork is needed to use 'System.Data.Entity').
using System.Linq; //NOTE: This is needed for IOrderedQueryable<Blog> type.
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //NOTE: It is needed to uniquely identify and entity in our class model. ([Key] inside of the 'User' class)

//NOTE: For this project I followed a tutorial from the microsoft websote called, "Code First to a New Database".
//      https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database

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
