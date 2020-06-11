# ModelFirstWebApplicationModel-First Web Application in ASP.NET Core MVC

using EntityFrameworkCore

![](media/image1.png){width="6.268055555555556in"
height="1.0291666666666666in"}

![](media/image2.png){width="6.268055555555556in"
height="4.345833333333333in"}

\(1\) Add EntityFrameworkCore to application (event hough it exists in
the following).

It\'s still best to get the latest version.

![](media/image3.png){width="6.268055555555556in"
height="1.729861111111111in"}

\(2\) We\'ll base our database on the following database model (n.b.
columns nenamed for better understanding)

![](media/image4.png){width="6.268055555555556in"
height="1.5479166666666666in"}

**\
**

**Customer.cs**

using System;

using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;

using System.Runtime.CompilerServices;

using System.Threading.Tasks;

namespace ModelFirstWebApplication.Models

{

    \[Table(\"Customer\")\]

    public class Customer

    {

        \[Key\]

        public int CustomerId { get; set; }

        \[Required\]

        public string Name { get; set; }

        public ICollection\<Order\> Orders { get; set; }

    }

}

**Order.cs**

using System;

using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;

using System.Threading.Tasks;

namespace ModelFirstWebApplication.Models

{

    \[Table(\"Order\")\]

    public class Order

    {

        \[Key\]

        public int OrderId { get; set; }

        \[Required\]

        \[DataType(DataType.Date)\]

        public string Date { get; set; }

        \[Required\]

        public string OrderState { get; set; }

        \[Required\]

        \[ForeignKey(\"Customer\")\]

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        

        public ICollection\<OrderDetail\> OrderDetails { get; set; }

    }

}

**\
**

**OrderDetails.cs**

using System;

using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;

using System.Threading.Tasks;

namespace ModelFirstWebApplication.Models

{

    \[Table(\"OrderDetail\")\]

    public class OrderDetail

    {

        \[Key\]

        public int OrderDetailId { get; set; }

        \[Required\]

        public int Count { get; set; }

        \[Required\]

        \[Column(TypeName = \"decimal(18,2)\")\]

        public decimal Amount { get; set; }

        \[Required\]

        \[ForeignKey(\"Order\")\]

        public int OrderId { get; set; }

        public Order Order { get; set; }

        \[Required\]

        \[ForeignKey(\"Product\")\]

        public int ProductId { get; set; }

        public Product Product { get; set; }

    }

}

**Product.cs**

using System;

using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;

using System.Threading.Tasks;

namespace ModelFirstWebApplication.Models

{

    \[Table(\"Product\")\]

    public class Product

    {

        \[Key\]

        public int ProductId { get; set; }

        

        \[Required\]

        public string Name { get; set; }

        public ICollection\<OrderDetail\> OrderDetails { get; set; }

    }

}

![](media/image5.png){width="6.268055555555556in"
height="2.5479166666666666in"}

\(3\) Create the SQL Server database (just the database - no tables,
these will come from Model First approach in ASP.NET Core MVC and Entity
Framework Core). Use SSMS (Sql Server Management Studio) to create the
database:

![](media/image6.png){width="4.2623764216972875in"
height="2.8137642169728783in"}

Create a new database

![](media/image7.png){width="5.545106080489939in"
height="5.024752843394576in"}

\(4\) Create the ConnectionString in appsettings.json

{

  \"Logging\": {

    \"LogLevel\": {

      \"Default\": \"Information\",

      \"Microsoft\": \"Warning\",

      \"Microsoft.Hosting.Lifetime\": \"Information\"

    }

  },

  \"ConnectionStrings\": {

    \"sqlConnection\": \"server=(localdb)\\\\MSSQLLocalDB; database=ModelFirst; Integrated Security=true\"

  },

  \"AllowedHosts\": \"\*\"

}

![](media/image8.png){width="6.268055555555556in"
height="1.7152777777777777in"}

\(5\) In the **ConfigureServices** method of **Startup.cs** we can
obtain the connection string through the IConfiguration interface done
through the IOC container.

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.SqlServer;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;

using ModelFirstWebApplication.Repository;

namespace ModelFirstWebApplication

{

    public class Startup

    {

        public Startup(IConfiguration configuration)

        {

            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)

        {

            services.AddDbContext\<RepositoryContext\>(opts =\> opts.UseSqlServer(Configuration.GetConnectionString(\"sqlConnection\")));

            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)

        {

            if (env.IsDevelopment())

            {

                app.UseDeveloperExceptionPage();

            }

            else

            {

                app.UseExceptionHandler(\"/Home/Error\");

            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =\>

            {

                endpoints.MapControllerRoute(

                    name: \"default\",

                    pattern: \"{controller=Home}/{action=Index}/{id?}\");

            });

        }

    }

}

These are the only changes that we need to make (highlighted)

![](media/image9.png){width="6.268055555555556in"
height="6.4215277777777775in"}

\(6\) Create the context class (RepositoryContext) which is the
middleware component for communication with the database. This
inherit\'s from EntityFrameworkCore\'s DbContext

**RepositoryContext.cs**

using Microsoft.EntityFrameworkCore;

using ModelFirstWebApplication.Models;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

namespace ModelFirstWebApplication.Repository

{

    public class RepositoryContext : DbContext

    {

        public RepositoryContext(DbContextOptions options) : base(options) { }

        public DbSet\<Customer\> Customer { get; set; }

        public DbSet\<Order\> Order { get; set; }

        public DbSet\<OrderDetail\> OrderDetail { get; set; }

        public DbSet\<Product\> Product { get; set; }

    }

}

![](media/image10.png){width="6.268055555555556in"
height="2.954861111111111in"}

\(6\) Build the database tables (including foreign keys and integrity)
using package manager console

**BEFORE**

![](media/image11.png){width="6.268055555555556in"
height="0.9069444444444444in"}

You do NOT need to enable-migrations; just do the following:

PM\> Add-Migration Initial -verbose

![](media/image12.png){width="6.268055555555556in"
height="3.0791666666666666in"}

![](media/image13.png){width="6.268055555555556in"
height="3.1680555555555556in"}

![](media/image14.png){width="2.062606080489939in"
height="0.38196412948381453in"}

using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelFirstWebApplication.Migrations

{

    public partial class Initial : Migration

    {

        protected override void Up(MigrationBuilder migrationBuilder)

        {

            migrationBuilder.CreateTable(

                name: \"Customer\",

                columns: table =\> new

                {

                    CustomerId = table.Column\<int\>(nullable: false)

                        .Annotation(\"SqlServer:Identity\", \"1, 1\"),

                    Name = table.Column\<string\>(nullable: false)

                },

                constraints: table =\>

                {

                    table.PrimaryKey(\"PK\_Customer\", x =\> x.CustomerId);

                });

            migrationBuilder.CreateTable(

                name: \"Product\",

                columns: table =\> new

                {

                    ProductId = table.Column\<int\>(nullable: false)

                        .Annotation(\"SqlServer:Identity\", \"1, 1\"),

                    Name = table.Column\<string\>(nullable: false)

                },

                constraints: table =\>

                {

                    table.PrimaryKey(\"PK\_Product\", x =\> x.ProductId);

                });

            migrationBuilder.CreateTable(

                name: \"Order\",

                columns: table =\> new

                {

                    OrderId = table.Column\<int\>(nullable: false)

                        .Annotation(\"SqlServer:Identity\", \"1, 1\"),

                    Date = table.Column\<string\>(nullable: false),

                    OrderState = table.Column\<string\>(nullable: false),

                    CustomerId = table.Column\<int\>(nullable: false)

                },

                constraints: table =\>

                {

                    table.PrimaryKey(\"PK\_Order\", x =\> x.OrderId);

                    table.ForeignKey(

                        name: \"FK\_Order\_Customer\_CustomerId\",

                        column: x =\> x.CustomerId,

                        principalTable: \"Customer\",

                        principalColumn: \"CustomerId\",

                        onDelete: ReferentialAction.Cascade);

                });

            migrationBuilder.CreateTable(

                name: \"OrderDetail\",

                columns: table =\> new

                {

                    OrderDetailId = table.Column\<int\>(nullable: false)

                        .Annotation(\"SqlServer:Identity\", \"1, 1\"),

                    Count = table.Column\<int\>(nullable: false),

                    Amount = table.Column\<decimal\>(type: \"decimal(18,2)\", nullable: false),

                    OrderId = table.Column\<int\>(nullable: false),

                    ProductId = table.Column\<int\>(nullable: false)

                },

                constraints: table =\>

                {

                    table.PrimaryKey(\"PK\_OrderDetail\", x =\> x.OrderDetailId);

                    table.ForeignKey(

                        name: \"FK\_OrderDetail\_Order\_OrderId\",

                        column: x =\> x.OrderId,

                        principalTable: \"Order\",

                        principalColumn: \"OrderId\",

                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(

                        name: \"FK\_OrderDetail\_Product\_ProductId\",

                        column: x =\> x.ProductId,

                        principalTable: \"Product\",

                        principalColumn: \"ProductId\",

                        onDelete: ReferentialAction.Cascade);

                });

            migrationBuilder.CreateIndex(

                name: \"IX\_Order\_CustomerId\",

                table: \"Order\",

                column: \"CustomerId\");

            migrationBuilder.CreateIndex(

                name: \"IX\_OrderDetail\_OrderId\",

                table: \"OrderDetail\",

                column: \"OrderId\");

            migrationBuilder.CreateIndex(

                name: \"IX\_OrderDetail\_ProductId\",

                table: \"OrderDetail\",

                column: \"ProductId\");

        }

        protected override void Down(MigrationBuilder migrationBuilder)

        {

            migrationBuilder.DropTable(

                name: \"OrderDetail\");

            migrationBuilder.DropTable(

                name: \"Order\");

            migrationBuilder.DropTable(

                name: \"Product\");

            migrationBuilder.DropTable(

                name: \"Customer\");

        }

    }

}

PM\> update-database -verbose

![](media/image15.png){width="6.268055555555556in" height="2.9in"}

**AFTER**

![](media/image16.png){width="6.268055555555556in"
height="4.2652777777777775in"}

Add Controllers (automatically) for each model

![](media/image17.png){width="6.268055555555556in"
height="3.6131944444444444in"}

![](media/image18.png){width="6.268055555555556in"
height="3.7715277777777776in"}

Now you can use the website (I amended the styliong using Bootstrap)

![](media/image19.png){width="6.268055555555556in"
height="3.7354166666666666in"}

On the Details page, I\'ve created a master-detail layout so you can see
the \'Customer Details\' and the \'Orders\' placed by that customer on
the same page

![](media/image20.png){width="6.268055555555556in"
height="4.029166666666667in"}

Similarly, here are the dedicated pages for the Orders

![](media/image21.png){width="6.268055555555556in"
height="4.041666666666667in"}

and the Order Details

![](media/image22.png){width="6.268055555555556in"
height="4.018055555555556in"}
