For every endpoint on BookController you will need a bearer token.
You can get one by posting to the login endpoint on DummyController (using swagger or postman) with the following credentials:
- Librarian (Admin) - u: librarian@library.com | p: librarianPassword
- Customer (Normal user) - u: customer@library.com | p: customerPassword

Use the bearer token with Postman against the BookController endpoints to get/modify the data.