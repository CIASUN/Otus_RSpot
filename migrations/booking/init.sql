CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Таблица Booking
CREATE TABLE IF NOT EXISTS booking (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    workspace_id UUID NOT NULL,
    user_id UUID NOT NULL,
    start_time TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    end_time TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT fk_booking_workspace FOREIGN KEY(workspace_id) REFERENCES workspace(id) ON DELETE CASCADE
);

-- Таблица WaitingList
CREATE TABLE IF NOT EXISTS waitinglist (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    workspace_id UUID NOT NULL,
    user_id UUID NOT NULL,
    start_time TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    end_time TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT fk_waitinglist_workspace FOREIGN KEY(workspace_id) REFERENCES workspace(id) ON DELETE CASCADE
);
