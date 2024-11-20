# Blood Bank Web API

This is a **Blood Bank Management System** developed using **ASP.NET Core Web API**. It allows users to perform CRUD operations on blood bank entries, as well as searching, filtering, pagination, and sorting functionalities.

## Features

- **CRUD Operations**: 
  - **Create**: Add new blood bank entries.
  - **Retrieve**: Fetch all blood bank entries or a specific entry by its ID.
  - **Update**: Modify existing blood bank entries.
  - **Delete**: Remove blood bank entries from the system.

- **Search Functionality**: 
  - By **Donor Name**: Case-insensitive and supports partial matches.
  - By **Blood Type**: Case-insensitive and exact matches.
  - By **Status**: Case-insensitive and partial matches.

- **Pagination**: 
  - Retrieve blood bank entries in a paginated format with customizable page size and page number.

- **Sorting**: 
  - Sort entries by `BloodType`, `CollectionDate`, or `ExpirationDate`, in ascending or descending order.

- **Filtering**: 
  - Filter entries based on multiple criteria such as `BloodType`, `Status`, and `DonorName`.

- **Validation**: 
  - Ensures that the blood bank entry is valid based on specific business rules like age, quantity, contact info, dates, and status.

## API Endpoints

- `POST /api/bloodbank`: Create a new blood bank entry.
- `GET /api/bloodbank`: Retrieve all blood bank entries.
- `GET /api/bloodbank/{id}`: Retrieve a specific blood bank entry by its ID.
- `PUT /api/bloodbank/{id}`: Update an existing blood bank entry.
- `DELETE /api/bloodbank/{id}`: Delete a blood bank entry by its ID.
- `GET /api/bloodbank/page`: Retrieve a paginated list of blood bank entries.
- `GET /api/bloodbank/search/donorname`: Search blood bank entries by donor name.
- `GET /api/bloodbank/search/bloodtype`: Search blood bank entries by blood type.
- `GET /api/bloodbank/search/status`: Search blood bank entries by status.
- `GET /api/bloodbank/sort`: Sort blood bank entries by a specified field.
- `GET /api/bloodbank/filter`: Filter blood bank entries based on multiple criteria.

## Data Model

### BloodBankEntry

- **Id**: The unique identifier for each blood bank entry.
- **DonorName**: Name of the blood donor.
- **Age**: Age of the donor (must be between 18 and 65).
- **BloodType**: The blood type of the donation (e.g., "A+", "O-", etc.).
- **ContactInfo**: The contact information (email) of the donor.
- **Quantity**: The amount of blood available (in milliliters).
- **CollectionDate**: The date the blood was collected.
- **ExpirationDate**: The date the blood donation expires.
- **Status**: The status of the blood bank entry (e.g., "Available", "Requested", or "Expired").
## **For more detailed explaination with screenshots, refer to the [documentation](https://github.com/swethashivannagari/BloodBankWebAPI/blob/master/BloodBankDoc.docx).**
