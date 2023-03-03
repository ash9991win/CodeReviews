README:

This is a small application mimicking the ones that are normally used in warehouses.
The idea is to help the warehouse worker pick more products in the least possible time.

Imagine a massive warehouse floor with dozens of aisles and hundreds of shelves. 
Customer orders are clubbed together in batches (typically 35-50 orders per batch)

Once the warehouse employee logs into the system (ideally a mobile app on a handheld device), 
he is assigned one batch of orders. At any point of time, the system should direct the employee 
to a product which is closest to his current location. This helps to optimize his efforts.

[In this application, Aisles are marked A to X and in each aisle, shelves are named a to j ]

For testing purposes, a small inventory of products is saved in a database (SQL Server). 

For the UI (using Windows Forms) - there are three forms: 

	A) Employee Login (frmLogin) 
	B) Product page (frmPickProduct) 
	C) Summary (frmSummary)

Steps:

	1 Login form authenticates the employee and takes him to the product page.
	2 On the product page, one product must be displayed at a time, with its location, no of units ordered, and a unique product id.

	3 A batch of orders contains around 30 products.
	4 The first product to be picked up must be the one with the least distance from where the employee is currently located. 
	5 Assume that the employee is initially located at X = 0 and Y = 0. (say the warehouse entrance).
	6 The app has to calculate the distances to all the products in the batch and choose the nearest possible one to be picked up first.

	7 Once the product is picked up, the employee has to enter its unique id (by means of scanning a code).
	8 Then the system:
		Should add this product to a Summary List (on the third page of our app).
		The next product to be picked up with its details must appear on the screen (of course, the nearest possible one). 
		(The distance calculation needs to happen dynamically on each step)

	9 Steps 6 through 8 to be repeated until all the products are picked up.

	10 The whole operation should be timed (I have not implemented this). 
	11 Timer helps to evaluate employee performance and to create useful comparison matrices.

	12 Finally, once the last product is picked up, the Summary Page should display all the products and their prices.
	13 On each step, the stock should be updated on the database.

    Notes:

	1 Ideally, the login credentials of the employee must be saved inside the database. 
	    For the time being, this is hard coded as:
		Username: abcd
		Password : 1234
	2 For the customer order, a batch of products from the stock (20% of the stock) is randomly generated using a stored procedure.
	3 For the quantity of each item, a number between 1 and 10 is randomly generated. 



I shall attach the code as text files
	3 Forms
	Product Class
	DBAccess Class
	Stored Procedure
	
Database design is in the spreadsheet. 
The warehouse layout and forms' design are shown in the pdf.


Thanks for your time and support!
