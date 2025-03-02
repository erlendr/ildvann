DROP TABLE IF EXISTS products;
CREATE TABLE IF NOT EXISTS products
(
    code                  TEXT PRIMARY KEY,
    name                  TEXT,
    main_category_name    TEXT,
    main_category_code    TEXT,
    main_subcategory_name TEXT,
    main_subcategory_code TEXT,
    main_country          TEXT,
    product_selection     TEXT
);

DROP TABLE IF EXISTS rumx_rums;
CREATE TABLE IF NOT EXISTS rumx_rums
(
    rxId        TEXT PRIMARY KEY,
    title       TEXT,
    img         TEXT,
    subtitle    TEXT,
    rating      DECIMAL,
    ratings     INTEGER,
    description TEXT,
    country     TEXT,
    url         TEXT,
    priceRange  INTEGER
);