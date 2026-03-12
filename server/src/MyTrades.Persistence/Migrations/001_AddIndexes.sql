-- 002_AddIndexes.sql

CREATE INDEX IF NOT EXISTS idx_candles_symbol_tf_time
    ON candles(symbol, timeframe, open_time DESC);

CREATE INDEX IF NOT EXISTS idx_candles_open_time
    ON candles(open_time DESC);