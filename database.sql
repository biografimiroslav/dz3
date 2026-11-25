-- ============================================================================
-- СИСТЕМА: "РЕСТОРАН" (Управління замовленнями, меню та клієнтами)
-- ============================================================================

DROP TABLE IF EXISTS order_items;
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS customers;
DROP TABLE IF EXISTS dishes;
DROP TABLE IF EXISTS categories;

-- ============================================================================
-- 1. ТАБЛИЦЯ: categories (Категорії меню)
-- ============================================================================
CREATE TABLE categories (
    category_id     INTEGER PRIMARY KEY AUTOINCREMENT,
    name            VARCHAR(100) NOT NULL,
    description     TEXT,
    is_active       BOOLEAN NOT NULL DEFAULT 1
);

-- ============================================================================
-- 2. ТАБЛИЦЯ: dishes (Страви / Меню ресторану)
-- ============================================================================
CREATE TABLE dishes (
    dish_id         INTEGER PRIMARY KEY AUTOINCREMENT,
    category_id     INTEGER NOT NULL,
    title           VARCHAR(150) NOT NULL,
    price           DECIMAL(10, 2) NOT NULL CHECK (price > 0),
    weight_grams    INTEGER NOT NULL CHECK (weight_grams > 0),
    calories        INTEGER CHECK (calories >= 0),
    is_available    BOOLEAN NOT NULL DEFAULT 1,
    created_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES categories (category_id) ON DELETE RESTRICT ON UPDATE CASCADE
);

-- ============================================================================
-- 3. ТАБЛИЦЯ: customers (Клієнти та програма лояльності)
-- ============================================================================
CREATE TABLE customers (
    customer_id         INTEGER PRIMARY KEY AUTOINCREMENT,
    full_name           VARCHAR(120) NOT NULL,
    phone               VARCHAR(20) NOT NULL UNIQUE,
    email               VARCHAR(100),
    discount_percent    DECIMAL(4, 2) NOT NULL DEFAULT 0.00 CHECK (discount_percent >= 0 AND discount_percent <= 100),
    registered_at       DATE NOT NULL DEFAULT CURRENT_DATE
);

-- ============================================================================
-- 4. ТАБЛИЦЯ: orders (Замовлення)
-- ============================================================================
CREATE TABLE orders (
    order_id        INTEGER PRIMARY KEY AUTOINCREMENT,
    customer_id     INTEGER,
    order_date      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    table_number    INTEGER CHECK (table_number > 0),
    total_price     DECIMAL(10, 2) NOT NULL DEFAULT 0.00 CHECK (total_price >= 0),
    status          VARCHAR(30) NOT NULL DEFAULT 'Нове' CHECK (status IN ('Нове', 'Готується', 'Виконано', 'Скасовано')),
    payment_method  VARCHAR(30) NOT NULL CHECK (payment_method IN ('Готівка', 'Безготівковий', 'Онлайн')),
    FOREIGN KEY (customer_id) REFERENCES customers (customer_id) ON DELETE SET NULL ON UPDATE CASCADE
);

-- ============================================================================
-- 5. ТАБЛИЦЯ: order_items (Позиції замовлення — зв'язок Many-to-Many)
-- ============================================================================
CREATE TABLE order_items (
    item_id         INTEGER PRIMARY KEY AUTOINCREMENT,
    order_id        INTEGER NOT NULL,
    dish_id         INTEGER NOT NULL,
    quantity        INTEGER NOT NULL DEFAULT 1 CHECK (quantity > 0),
    unit_price      DECIMAL(10, 2) NOT NULL CHECK (unit_price > 0),
    FOREIGN KEY (order_id) REFERENCES orders (order_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (dish_id) REFERENCES dishes (dish_id) ON DELETE RESTRICT ON UPDATE CASCADE
);

-- ============================================================================
-- НАПОВНЕННЯ ТЕСТОВИМИ ДАНИМИ (DML)
-- ============================================================================

INSERT INTO categories (name, description, is_active) VALUES
('Перші страви', 'Гарячі та холодні супи, борщі, бульйони', 1),
('Основні страви', 'М''ясні та рибні авторські страви', 1),
('Гарніри та салати', 'Овочеві салати та гарніри зі свіжих продуктів', 1),
('Десерти', 'Фірмова випічка, торти та тістечка', 1),
('Напої', 'Холодні лимонади, соки, кава та чай', 1);

INSERT INTO dishes (category_id, title, price, weight_grams, calories, is_available) VALUES
(1, 'Український борщ із пампушками', 185.00, 350, 320, 1),
(1, 'Крем-суп грибний із трюфельною олією', 195.00, 300, 280, 1),
(2, 'Стейк Рібай із соусом Деміглас', 480.00, 320, 650, 1),
(2, 'Філе лосося на грилі зі спаржею', 420.00, 280, 480, 1),
(2, 'Котлета по-київськи', 240.00, 250, 510, 1),
(3, 'Салат Цезар із куркою', 220.00, 260, 390, 1),
(3, 'Картопляне пюре з вершками', 85.00, 200, 210, 1),
(4, 'Баскський чизкейк із ягодами', 160.00, 180, 420, 1),
(4, 'Тірамісу класичний', 150.00, 160, 380, 1),
(5, 'Лимонад цитрусовий із м''ятою', 85.00, 400, 110, 1),
(5, 'Капучино на мигдалевому молоці', 75.00, 250, 90, 1);

INSERT INTO customers (full_name, phone, email, discount_percent, registered_at) VALUES
('Шевченко Мирослав Ігорович', '+380671112233', 'shevchenko@gmail.com', 10.00, '2026-01-15'),
('Коваленко Оксана Петрівна', '+380502223344', 'kovalenko.o@ukr.net', 5.00, '2026-02-10'),
('Мельник Андрій Васильович', '+380933334455', 'melnyk.a@gmail.com', 0.00, '2026-03-01'),
('Бондаренко Ірина Сергіївна', '+380984445566', 'iryna.b@gmail.com', 15.00, '2025-11-20');

INSERT INTO orders (customer_id, table_number, total_price, status, payment_method) VALUES
(1, 4, 775.00, 'Виконано', 'Безготівковий'),
(2, 7, 345.00, 'Виконано', 'Готівка'),
(3, 2, 480.00, 'Готується', 'Безготівковий'),
(NULL, 5, 260.00, 'Нове', 'Готівка');

INSERT INTO order_items (order_id, dish_id, quantity, unit_price) VALUES
(1, 3, 1, 480.00),
(1, 6, 1, 220.00),
(1, 11, 1, 75.00),
(2, 1, 1, 185.00),
(2, 8, 1, 160.00),
(3, 3, 1, 480.00),
(4, 1, 1, 185.00),
(4, 11, 1, 75.00);

-- ============================================================================
-- ЗАПИТИ ДЛЯ ПЕРЕВІРКИ СТРУКТУРИ ТА ЗВ'ЯЗКІВ
-- ============================================================================

-- 1. Перегляд повної інформації по замовленнях (з клієнтом та складом страв)
SELECT 
    o.order_id AS [ID Замовлення],
    IFNULL(c.full_name, 'Гість') AS [Клієнт],
    o.table_number AS [Стіл],
    d.title AS [Страва],
    oi.quantity AS [К-сть],
    oi.unit_price AS [Ціна за од.],
    (oi.quantity * oi.unit_price) AS [Сума рядка],
    o.status AS [Статус],
    o.payment_method AS [Оплата]
FROM orders o
LEFT JOIN customers c ON o.customer_id = c.customer_id
JOIN order_items oi ON o.order_id = oi.order_id
JOIN dishes d ON oi.dish_id = d.dish_id
ORDER BY o.order_id, d.title;
