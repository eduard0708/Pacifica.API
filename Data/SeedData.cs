using Pacifica.API.Data;
using Pacifica.API.Models;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API
{
    public class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure the database is created (applies migrations if needed)
            context.Database.Migrate();

            // Seed data for Branches (no Id specified)
            if (!context.Branches.Any())
            {
                context.Branches.AddRange(
                    new Branch { BranchName = "Main Branch" },
                    new Branch { BranchName = "North Branch" },
                    new Branch { BranchName = "South Branch" },
                    new Branch { BranchName = "East Branch" },
                    new Branch { BranchName = "West Branch" },
                    new Branch { BranchName = "Central Branch" },
                    new Branch { BranchName = "Suburban Branch" },
                    new Branch { BranchName = "City Center Branch" },
                    new Branch { BranchName = "Coastal Branch" },
                    new Branch { BranchName = "Mountain Branch" }  // New Branch
                );
                context.SaveChanges(); // Save branches to the database
            }

            // Seed data for Status (Product Status)
            if (!context.Statuses.Any())
            {
                context.Statuses.AddRange(
                    new Status { StatusName = "Available" },
                    new Status { StatusName = "OutOfStock" },
                    new Status { StatusName = "Discontinued" },
                    new Status { StatusName = "Pending" },
                    new Status { StatusName = "Backordered" },
                    new Status { StatusName = "PreOrder" },
                    new Status { StatusName = "Shipped" },
                    new Status { StatusName = "Reserved" },
                    new Status { StatusName = "SoldOut" },
                    new Status { StatusName = "ComingSoon" }
                );
                context.SaveChanges(); // Save statuses
            }

            // Seed data for Categories
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { CategoryName = "Fish Foods" },
                    new Category { CategoryName = "Aquarium Accessories" },
                    new Category { CategoryName = "Hog Feeds" },
                    new Category { CategoryName = "Chicken Feeds" },
                    new Category { CategoryName = "Bird Feeds" },
                    new Category { CategoryName = "Dog Foods" },
                    new Category { CategoryName = "Cat Foods" },
                    new Category { CategoryName = "Pet Toys" },
                    new Category { CategoryName = "Aquarium Equipment" },
                    new Category { CategoryName = "Small Animal Feeds" }
                );
                context.SaveChanges(); // Save categories
            }

            if (!context.StockInReferences.Any())
            {
                context.StockInReferences.AddRange(
                    new StockInReference { StockInReferenceName = "Supplier BMEG" },
                    new StockInReference { StockInReferenceName = "Supplier Excel Feeds" },
                    new StockInReference { StockInReferenceName = "Supplier Robina" },
                    new StockInReference { StockInReferenceName = "Kalibo Branch" },
                    new StockInReference { StockInReferenceName = "Roxas Branch" }
                );
                context.SaveChanges(); // Save categories
            }

            if (!context.StockOutReferences.Any())
            {
                context.StockOutReferences.AddRange(
                    new StockOutReference { StockOutReferenceName = "Sold in Store" },
                    new StockOutReference { StockOutReferenceName = "Transfer Kalibo Branch" },
                    new StockOutReference { StockOutReferenceName = "Transfer Roxas Branch" },
                    new StockOutReference { StockOutReferenceName = "Transfer Iloilo Branch" },
                    new StockOutReference { StockOutReferenceName = "Damage Disposed" }
                );
                context.SaveChanges(); // Save categories
            }


            if (!context.PaymentMethods.Any())
            {
                context.PaymentMethods.AddRange(
                    new PaymentMethod { PaymentMethodName = "Cash" },
                    new PaymentMethod { PaymentMethodName = "Charge" }
                    
                );
                context.SaveChanges(); // Save categories
            }




            // Seed data for Suppliers
            if (!context.Suppliers.Any())
            {
                context.Suppliers.AddRange(
                    new Supplier { SupplierName = "AKFF AKWARYUM PETS" },
                    new Supplier { SupplierName = "AQUA GOLD TRADING/AQUATINUM CORP" },
                    new Supplier { SupplierName = "ASVET INC." },
                    new Supplier { SupplierName = "BELMAN LABORATORIES" },
                    new Supplier { SupplierName = "GENERAL ANIMAL FEED & NUTRITION" },
                    new Supplier { SupplierName = "PETCO SUPPLY" },
                    new Supplier { SupplierName = "PETSMART INC." },
                    new Supplier { SupplierName = "TROPICAL AQUATICS LTD" },
                    new Supplier { SupplierName = "AQUATIC NATURE" },
                    new Supplier { SupplierName = "MARINE AQUATICS SUPPLY" }
                );
                context.SaveChanges(); // Save suppliers
            }

            // Seed data for Products
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                new Product { ProductName = "Fish Food A", SKU = "SKU001", CategoryId = 1, SupplierId = 1, Remarks = "High-quality fish food", CreatedBy = "Admin" },
                new Product { ProductName = "Aquarium Filter", SKU = "SKU002", CategoryId = 2, SupplierId = 2, Remarks = "Efficient filtration", CreatedBy = "Admin" },
                new Product { ProductName = "Hog Feed B", SKU = "SKU003", CategoryId = 3, SupplierId = 3, Remarks = "Specialized feed for hogs", CreatedBy = "Admin" },
                new Product { ProductName = "Chicken Feed C", SKU = "SKU004", CategoryId = 4, SupplierId = 4, Remarks = "Nutritious feed for chickens", CreatedBy = "Admin" },
                new Product { ProductName = "Bird Feed D", SKU = "SKU005", CategoryId = 5, SupplierId = 5, Remarks = "Suitable for various bird species", CreatedBy = "Admin" },
                new Product { ProductName = "Dog Food E", SKU = "SKU006", CategoryId = 6, SupplierId = 6, Remarks = "Healthy food for dogs", CreatedBy = "Admin" },
                new Product { ProductName = "Cat Food F", SKU = "SKU007", CategoryId = 7, SupplierId = 7, Remarks = "Premium food for cats", CreatedBy = "Admin" },
                new Product { ProductName = "Pet Toy G", SKU = "SKU008", CategoryId = 8, SupplierId = 8, Remarks = "Durable toy for pets", CreatedBy = "Admin" },
                new Product { ProductName = "Aquarium Heater H", SKU = "SKU009", CategoryId = 9, SupplierId = 9, Remarks = "Heater for maintaining water temperature", CreatedBy = "Admin" },
                new Product { ProductName = "Small Animal Feed I", SKU = "SKU010", CategoryId = 10, SupplierId = 10, Remarks = "Food for small animals", CreatedBy = "Admin" }

                );
                context.SaveChanges(); // Save products
            }

            // Seed data for BranchProduct (assign 10 products to each branch)
            var branches = context.Branches.ToList();
            var products = context.Products.ToList();
            var statusId = 1;  // Assuming 1 represents "Available" status
            var createdBy = "Admin";  // Assuming the user is "Admin"

            foreach (var branch in branches)
            {
                // Add 10 products for each branch
                for (int i = 0; i < 10; i++)
                {
                    context.BranchProducts.Add(new BranchProduct
                    {
                        BranchId = branch.Id,
                        ProductId = products[i % products.Count].Id,  // Loop through products using modulus
                        StatusId = statusId,
                        CostPrice = 10.00m,  // Sample price
                        RetailPrice = 15.00m,  // Sample retail price
                        StockQuantity = 100,  // Sample stock quantity
                        ReorderLevel = 10,  // Sample reorder level
                        MinStockLevel = 5,  // Sample minimum stock level
                        Remarks = "From Seed Data",
                        CreatedAt = DateTime.UtcNow,  // Use current UTC time
                        CreatedBy = createdBy
                    });
                }
            }

            // Save BranchProduct records
            context.SaveChanges();
        }
    }
}
