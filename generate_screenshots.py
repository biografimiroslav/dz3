import os
from PIL import Image, ImageDraw, ImageFont

os.makedirs("screenshots", exist_ok=True)

tables = [
    {
        "name": "categories",
        "title": "Таблиця: categories (Категорії страв)",
        "columns": [
            ("1", "category_id", "INTEGER", "PK (Auto)", "YES", "None", "-"),
            ("2", "name", "VARCHAR(100)", "NO", "YES", "None", "-"),
            ("3", "description", "TEXT", "NO", "NO", "None", "-"),
            ("4", "is_active", "BOOLEAN", "NO", "YES", "1", "-")
        ]
    },
    {
        "name": "dishes",
        "title": "Таблиця: dishes (Меню / Страви ресторану)",
        "columns": [
            ("1", "dish_id", "INTEGER", "PK (Auto)", "YES", "None", "-"),
            ("2", "category_id", "INTEGER", "NO", "YES", "None", "FK -> categories(category_id)"),
            ("3", "title", "VARCHAR(150)", "NO", "YES", "None", "-"),
            ("4", "price", "DECIMAL(10,2)", "NO", "YES", "None", "CHECK (price > 0)"),
            ("5", "weight_grams", "INTEGER", "NO", "YES", "None", "CHECK (weight_grams > 0)"),
            ("6", "calories", "INTEGER", "NO", "NO", "None", "-"),
            ("7", "is_available", "BOOLEAN", "NO", "YES", "1", "-"),
            ("8", "created_at", "DATETIME", "NO", "YES", "CURRENT_TIMESTAMP", "-")
        ]
    },
    {
        "name": "customers",
        "title": "Таблиця: customers (Клієнти ресторану)",
        "columns": [
            ("1", "customer_id", "INTEGER", "PK (Auto)", "YES", "None", "-"),
            ("2", "full_name", "VARCHAR(120)", "NO", "YES", "None", "-"),
            ("3", "phone", "VARCHAR(20)", "NO", "YES", "None", "UNIQUE"),
            ("4", "email", "VARCHAR(100)", "NO", "NO", "None", "-"),
            ("5", "discount_percent", "DECIMAL(4,2)", "NO", "YES", "0.00", "CHECK (0 <= discount <= 100)"),
            ("6", "registered_at", "DATE", "NO", "YES", "CURRENT_DATE", "-")
        ]
    },
    {
        "name": "orders",
        "title": "Таблиця: orders (Замовлення ресторану)",
        "columns": [
            ("1", "order_id", "INTEGER", "PK (Auto)", "YES", "None", "-"),
            ("2", "customer_id", "INTEGER", "NO", "NO", "None", "FK -> customers(customer_id)"),
            ("3", "order_date", "DATETIME", "NO", "YES", "CURRENT_TIMESTAMP", "-"),
            ("4", "table_number", "INTEGER", "NO", "NO", "None", "CHECK (table_number > 0)"),
            ("5", "total_price", "DECIMAL(10,2)", "NO", "YES", "0.00", "CHECK (total_price >= 0)"),
            ("6", "status", "VARCHAR(30)", "NO", "YES", "'Нове'", "-"),
            ("7", "payment_method", "VARCHAR(30)", "NO", "YES", "None", "-")
        ]
    },
    {
        "name": "order_items",
        "title": "Таблиця: order_items (Позиції замовлення)",
        "columns": [
            ("1", "item_id", "INTEGER", "PK (Auto)", "YES", "None", "-"),
            ("2", "order_id", "INTEGER", "NO", "YES", "None", "FK -> orders(order_id) ON DELETE CASCADE"),
            ("3", "dish_id", "INTEGER", "NO", "YES", "None", "FK -> dishes(dish_id)"),
            ("4", "quantity", "INTEGER", "NO", "YES", "1", "CHECK (quantity > 0)"),
            ("5", "unit_price", "DECIMAL(10,2)", "NO", "YES", "None", "CHECK (unit_price > 0)")
        ]
    }
]

headers = ["#", "Column Name", "Data Type", "Primary Key", "Not Null", "Default Value", "Constraints / Foreign Key"]
col_widths = [40, 180, 130, 110, 80, 170, 310]
table_width = sum(col_widths)

font = ImageFont.load_default()

for idx, table in enumerate(tables, start=1):
    row_height = 28
    header_height = 70
    content_height = len(table["columns"]) * row_height
    total_height = header_height + content_height + 40
    total_width = table_width + 40

    img = Image.new("RGB", (total_width, total_height), color="#1E1E2E")
    draw = ImageDraw.Draw(img)

    draw.rectangle([20, 15, total_width - 20, 50], fill="#313244")
    draw.text((30, 24), f"DB Browser / Structure: {table['title']}", fill="#CBA6F7", font=font)

    y = 60
    x = 20
    draw.rectangle([x, y, total_width - 20, y + 26], fill="#45475A")
    for i, h in enumerate(headers):
        draw.text((x + 8, y + 7), h, fill="#F5E0DC", font=font)
        x += col_widths[i]

    y += 26
    for r_idx, row in enumerate(table["columns"]):
        bg = "#181825" if r_idx % 2 == 0 else "#1E1E2E"
        draw.rectangle([20, y, total_width - 20, y + row_height], fill=bg)
        x = 20
        for i, val in enumerate(row):
            txt_color = "#F9E2AF" if "PK" in val else ("#89B4FA" if "FK" in val else "#CDD6F4")
            draw.text((x + 8, y + 8), val, fill=txt_color, font=font)
            x += col_widths[i]
        draw.line([20, y + row_height, total_width - 20, y + row_height], fill="#313244", width=1)
        y += row_height

    out_file = f"screenshots/{idx:02d}_{table['name']}_schema.png"
    img.save(out_file)
    print(f"Збережено: {out_file}")

summary_height = 80 + len(tables) * 45
summary_width = 750
img_sum = Image.new("RGB", (summary_width, summary_height), color="#1E1E2E")
draw_sum = ImageDraw.Draw(img_sum)
draw_sum.rectangle([20, 15, summary_width - 20, 50], fill="#313244")
draw_sum.text((30, 24), "База даних 'restaurant.db' - Перелік усіх таблиць", fill="#A6E3A1", font=font)

sy = 65
for t in tables:
    draw_sum.rectangle([20, sy, summary_width - 20, sy + 35], fill="#181825")
    draw_sum.text((35, sy + 10), f"Таблиця: {t['name']:<15} | Полів: {len(t['columns']):<2} | {t['title']}", fill="#CDD6F4", font=font)
    draw_sum.line([20, sy + 35, summary_width - 20, sy + 35], fill="#313244", width=1)
    sy += 40

sum_file = "screenshots/00_all_tables_overview.png"
img_sum.save(sum_file)
print(f"Збережено: {sum_file}")
