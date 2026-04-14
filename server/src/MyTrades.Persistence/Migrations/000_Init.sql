CREATE TABLE IF NOT EXISTS symbols
(
    id          BIGSERIAL PRIMARY KEY,
    name        VARCHAR(20) NOT NULL UNIQUE,
    created_at  TIMESTAMPTZ NOT NULL DEFAULT NOW()
    );

CREATE TABLE IF NOT EXISTS candles
(
    id          BIGSERIAL PRIMARY KEY,
    symbol_id   BIGINT NOT NULL,
    timeframe   VARCHAR(10) NOT NULL,
    open_time   TIMESTAMPTZ NOT NULL,
    close_time  TIMESTAMPTZ NOT NULL,
    open_price  NUMERIC(18, 8) NOT NULL,
    high_price  NUMERIC(18, 8) NOT NULL,
    low_price   NUMERIC(18, 8) NOT NULL,
    close_price NUMERIC(18, 8) NOT NULL,
    volume      NUMERIC(20, 8) NOT NULL,
    trade_count INTEGER DEFAULT 0,
    created_at  TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT uq_candle UNIQUE (symbol_id, timeframe, open_time),
    CONSTRAINT fk_symbol FOREIGN KEY (symbol_id) REFERENCES symbols (id) ON DELETE CASCADE
    );