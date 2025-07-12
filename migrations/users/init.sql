CREATE EXTENSION IF NOT EXISTS "pgcrypto";
CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    name TEXT NOT NULL,
    role TEXT NOT NULL DEFAULT 'User',
    created_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

-- Предзаполнение начальными пользователями
INSERT INTO users (id, email, password_hash, name, role)
VALUES 
    (
        gen_random_uuid(),
        'admin@example.com',
        'admin123hash',         -- заменить на реальный хэш
        'Admin',
        'Admin'
    ),
    (
        gen_random_uuid(),
        'user1@example.com',
        'user1hash',            -- заменить на реальный хэш
        'User One',
        'User'
    )
ON CONFLICT (email) DO NOTHING;
