[Back to README](../README.md)

### Sales

All responses are wrapped in the standard API envelope:

```json
{
  "data": { },
  "success": true,
  "message": "string",
  "errors": []
}
```

A sale references its customer, branch and products through the External Identities pattern:
the identifier from the owning domain is stored together with a denormalized name.

Discounts are applied per item based on the quantity of identical items:

- Below 4 items: no discount
- From 4 to 9 items: 10%
- From 10 to 20 items: 20%
- Above 20 items: not allowed

#### POST /api/sales
- Description: Create a new sale. The item discounts and the sale total are calculated by the API.
- Request Body:
  ```json
  {
    "saleNumber": "string",
    "saleDate": "2026-07-03T00:00:00Z",
    "customerId": "guid",
    "customerName": "string",
    "branchId": "guid",
    "branchName": "string",
    "items": [
      {
        "productId": "guid",
        "productName": "string",
        "quantity": "integer",
        "unitPrice": "number"
      }
    ]
  }
  ```
- Response (`201 Created`):
  ```json
  {
    "data": {
      "id": "guid",
      "saleNumber": "string",
      "saleDate": "2026-07-03T00:00:00Z",
      "customerId": "guid",
      "customerName": "string",
      "branchId": "guid",
      "branchName": "string",
      "totalAmount": "number",
      "isCancelled": false,
      "items": [
        {
          "id": "guid",
          "productId": "guid",
          "productName": "string",
          "quantity": "integer",
          "unitPrice": "number",
          "discount": "number",
          "total": "number",
          "isCancelled": false
        }
      ]
    },
    "success": true,
    "message": "Sale created successfully",
    "errors": []
  }
  ```

#### GET /api/sales
- Description: Retrieve a page of sales.
- Query Parameters:
  - `_page` (optional): Page number (default: 1)
  - `_size` (optional): Items per page (default: 10, maximum: 50)
  - `_order` (optional): Ordering of results, for example `saleDate desc, totalAmount asc`. Allowed fields: `saleNumber`, `saleDate`, `totalAmount`, `customerName`, `branchName`.
  - `customerId` (optional): Filter by customer.
  - `branchId` (optional): Filter by branch.
  - `isCancelled` (optional): Filter by cancelled state (`true`/`false`).
  - `_minSaleDate` / `_maxSaleDate` (optional): Filter by sale date range.
  - `_minTotalAmount` / `_maxTotalAmount` (optional): Filter by total amount range.
- Response (`200 OK`):
  ```json
  {
    "data": [
      {
        "id": "guid",
        "saleNumber": "string",
        "saleDate": "2026-07-03T00:00:00Z",
        "customerId": "guid",
        "customerName": "string",
        "branchId": "guid",
        "branchName": "string",
        "totalAmount": "number",
        "isCancelled": false,
        "items": []
      }
    ],
    "currentPage": "integer",
    "totalPages": "integer",
    "totalCount": "integer",
    "success": true
  }
  ```

#### GET /api/sales/{id}
- Description: Retrieve a specific sale by its identifier.
- Path Parameters:
  - `id`: Sale identifier.
- Response (`200 OK`): the sale, using the same shape as the create response `data`.
- Response (`404 Not Found`) when the sale does not exist.

#### PUT /api/sales/{id}
- Description: Update a sale. The items are replaced and the discounts and total are recalculated.
- Path Parameters:
  - `id`: Sale identifier.
- Request Body: same shape as the create request body.
- Response (`200 OK`): the updated sale.

#### DELETE /api/sales/{id}
- Description: Delete a sale.
- Path Parameters:
  - `id`: Sale identifier.
- Response (`200 OK`):
  ```json
  {
    "success": true,
    "message": "Sale deleted successfully",
    "errors": []
  }
  ```

#### POST /api/sales/{id}/cancel
- Description: Cancel a whole sale. Publishes the `SaleCancelled` event.
- Path Parameters:
  - `id`: Sale identifier.
- Response (`200 OK`): the sale with `isCancelled` set to `true`.

#### POST /api/sales/{saleId}/items/{itemId}/cancel
- Description: Cancel a single item of a sale. The sale total is recalculated and the `ItemCancelled` event is published.
- Path Parameters:
  - `saleId`: Sale identifier.
  - `itemId`: Item identifier.
- Response (`200 OK`): the sale with the item flagged as cancelled.

#### Events

The API publishes `SaleCreated`, `SaleModified`, `SaleCancelled` and `ItemCancelled` events.
As allowed by the challenge, they are written to the application log instead of a message broker.

<br/>
<div style="display: flex; justify-content: space-between;">
  <a href="./general-api.md">Previous: General API</a>
</div>
