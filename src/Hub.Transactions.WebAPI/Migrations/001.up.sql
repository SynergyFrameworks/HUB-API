-- Sequence: public.cs_rpt_txn_seq

CREATE SEQUENCE IF NOT EXISTS public.cs_rpt_txn_seq
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 36580
  CACHE 1;
ALTER TABLE public.cs_rpt_txn_seq
  OWNER TO postgres;


-- Table: public.cs_rpt_txn


CREATE TABLE IF NOT EXISTS public.cs_rpt_txn
(
  cs_rpt_txn_id integer NOT NULL DEFAULT nextval('cs_rpt_txn_seq'::regclass),
  mongo_m_id character varying(100),
  corp_name character varying(100),
  merchant_name character varying(100),
  userid character varying(50),
  mid character varying(50),
  resp_code character varying(100),
  resp_code_desc character varying(500),
  mongo_bank_id character varying(100),
  bank_id character varying(100),
  bank_name character varying(100),
  card_type character varying(20),
  bin character varying(100),
  bin_country character varying(50),
  txn_type character varying(50),
  m_txn_id character varying(50),
  apc_txn_id character varying(50),
  method_name character varying(20),
  channel_type character varying(20),
  currency character varying(50),
  amount bigint,
  customer_ip_address character varying(50),
  customer_email_address character varying(50),
  acc_holder character varying(50),
  customer_name character varying(50),
  req_bank_name character varying(50),
  req_bank_id character varying(50),
  billing_address_country character varying(50),
  req_rcv_at_main_flw timestamp without time zone,
  res_snt_back_to_mrchnt timestamp without time zone,
  req_snt_to_web_srvc timestamp without time zone,
  res_rcv_frm_web_srvc timestamp without time zone,
  apc_req_data text,
  apc_resp_data text,
  card_num character varying,
  exp_year character varying,
  exp_month character varying,
  exch_rate character varying(100),
  exch_amount character varying(100),
  exch_currency character varying(100),
  crypto_key character varying(100),
  bank_acc_holder character varying(50),
  intl_bank_code character varying(50),
  bank_account_number character varying(50),
  pre_approval_status character varying(50),
  card_last_four_digits character varying(20),
  customer_tel_no character varying(50),
  customer_id character varying(50),
  customer_date_of_birth character varying(50),
  customer_social_id character varying(50),
  billing_address_customer_name character varying(50),
  billing_address_street1 character varying(100),
  billing_address_city character varying(50),
  shipping_address_customer_name character varying(50),
  shipping_address_street1 character varying(100),
  shipping_address_city character varying(50),
  shipping_address_country character varying(50),
  provider_reference character varying(50),
  corp_id character varying(100),
  CONSTRAINT cs_rpt_txn_id_pk PRIMARY KEY (cs_rpt_txn_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cs_rpt_txn
  OWNER TO postgres;



-- Sequence: public.cs_rpt_timestamp_seq

CREATE SEQUENCE IF NOT EXISTS public.cs_rpt_timestamp_seq
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 68973
  CACHE 1;
ALTER TABLE public.cs_rpt_timestamp_seq
  OWNER TO postgres;



-- Table: public.cs_rpt_timestamp

CREATE TABLE IF NOT EXISTS public.cs_rpt_timestamp
(
  id integer NOT NULL DEFAULT nextval('cs_rpt_timestamp_seq'::regclass),
  cs_rpt_txn_id integer,
  api_name character varying(30),
  api_id character varying(100),
  resp_code character varying(10),
  resp_code_desc character varying(500),
  provider_resp_code character varying(200),
  provider_resp_code_desc character varying(500),
  req_snt_to_sub_flw timestamp without time zone,
  res_rcv_frm_sub_flw timestamp without time zone,
  req_snt_to_ext_api timestamp without time zone,
  res_rcv_frm_ext_api timestamp without time zone,
  req_rcv_at_sub_flw timestamp without time zone,
  res_snt_to_main_flw timestamp without time zone,
  CONSTRAINT cs_rpt_timestamp_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cs_rpt_timestamp
  OWNER TO postgres;


-- Table: public.bin_country_details_v

CREATE TABLE IF NOT EXISTS public.bin_country_details_v
(
  bin character varying(20) NOT NULL,
  card_brand character varying(20),
  issuing_org character varying(100), -- Issuing Organization
  card_type character varying(20), -- Type of Card (DEBIT, CREDIT, or CHARGE CARD),
  card_category character varying(50), -- Category of Card,
  country_name character varying(200), -- issuing country ISO name,
  country_a2_code character varying(6), -- issuing country ISO A2 code,
  country_a3_code character varying(10), -- issuing country ISO A3 code,
  country_iso_num numeric, -- issuing country ISO number,
  org_url character varying(200), -- issuing organization URL or the URL of the website that contains any relevant information.
  org_phone character varying(200), -- bank/issuing organization phone number. Optional field.
  CONSTRAINT bin_country_details_v_pkey PRIMARY KEY (bin)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.bin_country_details_v
  OWNER TO postgres;
COMMENT ON COLUMN public.bin_country_details_v.issuing_org IS 'Issuing Organization';
COMMENT ON COLUMN public.bin_country_details_v.card_type IS 'Type of Card (DEBIT, CREDIT, or CHARGE CARD),';
COMMENT ON COLUMN public.bin_country_details_v.card_category IS 'Category of Card,';
COMMENT ON COLUMN public.bin_country_details_v.country_name IS 'issuing country ISO name,';
COMMENT ON COLUMN public.bin_country_details_v.country_a2_code IS 'issuing country ISO A2 code,';
COMMENT ON COLUMN public.bin_country_details_v.country_a3_code IS 'issuing country ISO A3 code,';
COMMENT ON COLUMN public.bin_country_details_v.country_iso_num IS 'issuing country ISO number,';
COMMENT ON COLUMN public.bin_country_details_v.org_url IS 'issuing organization URL or the URL of the website that contains any relevant information. ';
COMMENT ON COLUMN public.bin_country_details_v.org_phone IS 'bank/issuing organization phone number. Optional field.';


-- Index: public.country_a2_code_idx

CREATE INDEX IF NOT EXISTS country_a2_code_idx
  ON public.bin_country_details_v
  USING btree
  (country_a2_code COLLATE pg_catalog."default");



-- Table: public.cs_crypto_key

CREATE TABLE IF NOT EXISTS public.cs_crypto_key
(
  crypto_id character varying(100) NOT NULL,
  crypto_key character varying(100) NOT NULL,
  CONSTRAINT cs_crypto_key_id_pk PRIMARY KEY (crypto_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cs_crypto_key
  OWNER TO postgres;



-- Sequence: public.cs_emailage_txn_info_id_seq1

CREATE SEQUENCE IF NOT EXISTS public.cs_emailage_txn_info_id_seq1
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 338
  CACHE 1;
ALTER TABLE public.cs_emailage_txn_info_id_seq1
  OWNER TO postgres;


-- Table: public.cs_emailage_txn_info

CREATE TABLE IF NOT EXISTS public.cs_emailage_txn_info
(
  id integer NOT NULL DEFAULT nextval('cs_emailage_txn_info_id_seq1'::regclass),
  mer_req_timestamp timestamp without time zone,
  req_recv_at timestamp without time zone,
  res_sent_at timestamp without time zone,
  api_resp_code character varying(100),
  api_resp_msg character varying(100),
  apc_resp_code character varying(100),
  apc_resp_msg character varying(100),
  apc_req_data text,
  apc_resp_data text,
  api_req_data text,
  api_resp_data text,
  m_txn_id character varying(100),
  txn_type_id character varying(100),
  email character varying(100),
  name character varying(100),
  domain_age character varying(100),
  first_verification_date character varying(20),
  last_verification_date character varying(20),
  status character varying(100),
  country character varying(100),
  fraud_risk character varying(100),
  ea_score character varying(100),
  ea_reason character varying(200),
  ea_advice character varying(200),
  ea_risk_band_id character varying(100),
  ea_risk_band character varying(100),
  dob character varying(50),
  location character varying(100),
  sm_friends character varying(100),
  unique_hits character varying(100),
  email_exists character varying(100),
  domain_exists character varying(100),
  company character varying(100),
  CONSTRAINT cs_emailage_txn_info_pk PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cs_emailage_txn_info
  OWNER TO postgres;


-- Table: public.cs_fail_txn_info

CREATE TABLE IF NOT EXISTS public.cs_fail_txn_info
(

)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cs_fail_txn_info
  OWNER TO postgres;
