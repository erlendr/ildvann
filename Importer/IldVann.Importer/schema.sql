-- a schema for a database table called products
-- that will store the following fields:
-- code (primary key, string)
-- name (string)
-- main_category_name (string)
-- main_category_code (string)
-- main_subcategory_name (string)
-- main_subcategory_code (string)
-- main_country (string)
-- product_selection (string)


-- drop the table if it exists before creating it
DROP TABLE IF EXISTS products;
CREATE TABLE IF NOT EXISTS products (
    code TEXT PRIMARY KEY,
    name TEXT,
    main_category_name TEXT,
    main_category_code TEXT,
    main_subcategory_name TEXT,
    main_subcategory_code TEXT,
    main_country TEXT,
    product_selection TEXT
);
