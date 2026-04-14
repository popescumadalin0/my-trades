CREATE TABLE dead_letters (
                              id          BIGSERIAL PRIMARY KEY,
                              event_type  VARCHAR(255) NOT NULL,
                              event_data  JSONB NOT NULL,
                              handler     VARCHAR(255) NOT NULL,
                              error       TEXT NOT NULL,
                              attempts    INTEGER NOT NULL,
                              created_at  TIMESTAMPTZ NOT NULL DEFAULT NOW()
);