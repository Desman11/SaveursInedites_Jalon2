-- Création du schéma public si nécessaire (en général déjà présent par défaut)
CREATE SCHEMA IF NOT EXISTS public;

-- On définit le schéma public comme schéma par défaut pour les objets créés ensuite
SET search_path TO public;

-- Début de la transaction pour exécuter tout le script de manière atomique
BEGIN;

-- (Optionnel en dev) Nettoyage pour pouvoir rejouer le script sans erreur
-- On supprime d'abord les tables dépendantes, puis les tables principales
DROP TABLE IF EXISTS categories_recettes CASCADE;
DROP TABLE IF EXISTS categories CASCADE;
DROP TABLE IF EXISTS ingredients_recettes CASCADE;
DROP TABLE IF EXISTS ingredients CASCADE;
DROP TABLE IF EXISTS etapes CASCADE;
DROP TABLE IF EXISTS avis CASCADE;
DROP TABLE IF EXISTS recette CASCADE;
DROP TABLE IF EXISTS utilisateurs CASCADE;

-----------------------------------------------------------
-- Création des tables
-----------------------------------------------------------

-- Table des utilisateurs (comptes de la plateforme)
CREATE TABLE utilisateurs (
    id SERIAL PRIMARY KEY,              -- Identifiant unique auto-incrémenté
    identifiant VARCHAR(20) UNIQUE NOT NULL, -- Nom d’utilisateur (login) unique
    email VARCHAR(50) UNIQUE NOT NULL,  -- Email unique pour chaque utilisateur
    password VARCHAR(255) NOT NULL      -- Mot de passe (hash) stocké en base
);

-- Table principale des recettes
CREATE TABLE recette (
    id SERIAL PRIMARY KEY,                     -- Identifiant unique de la recette
    nom VARCHAR(100) NOT NULL,                -- Nom de la recette
    temps_preparation INTERVAL NOT NULL,      -- Durée de préparation
    temps_cuisson INTERVAL NOT NULL DEFAULT '00:00:00', -- Durée de cuisson (0 par défaut)
    difficulte INT NOT NULL CHECK (difficulte BETWEEN 1 AND 5), -- Niveau de difficulté 1 à 5
    photo VARCHAR(100),                       -- Nom/chemin de la photo associée
    createur INT REFERENCES utilisateurs(id) ON DELETE CASCADE
    -- Clé étrangère: utilisateur créateur de la recette, suppression en cascade
);

-- Table des avis laissés par les utilisateurs sur une recette
CREATE TABLE avis (
    id_recette INT NOT NULL REFERENCES recette(id) ON DELETE CASCADE, 
    -- Référence à la recette, suppression en cascade si la recette disparaît
    id_utilisateur INT NOT NULL REFERENCES utilisateurs(id) ON DELETE CASCADE,
    -- Référence à l’utilisateur ayant laissé l’avis
    note INT NOT NULL CHECK (note BETWEEN 1 AND 5), -- Note de 1 à 5
    commentaire VARCHAR(500),                       -- Commentaire texte
    PRIMARY KEY (id_recette, id_utilisateur)       -- Un avis par utilisateur et par recette
);

-- Table des étapes de préparation d’une recette
CREATE TABLE etapes (
    numero INT NOT NULL,                            -- Numéro de l’étape (ordre)
    id_recette INT NOT NULL REFERENCES recette(id) ON DELETE CASCADE,
    -- Référence à la recette concernée
    texte VARCHAR(500) NOT NULL,                    -- Description de l’étape
    PRIMARY KEY (numero, id_recette)                -- Une étape n par recette
);

-- Table de référence des ingrédients possibles
CREATE TABLE ingredients (
    id SERIAL PRIMARY KEY,           -- Identifiant unique de l’ingrédient
    nom VARCHAR(50) UNIQUE NOT NULL  -- Nom de l’ingrédient, unique
);

-- Table de jonction Recette <-> Ingrédients (relation N-N)
CREATE TABLE ingredients_recettes (
    id_ingredient INT NOT NULL REFERENCES ingredients(id) ON DELETE CASCADE,
    -- Référence à l’ingrédient
    id_recette INT NOT NULL REFERENCES recette(id) ON DELETE CASCADE,
    -- Référence à la recette
    quantite VARCHAR(40) NOT NULL,   -- Quantité de l’ingrédient (ex: "200 g", "2 c.à.s.")
    PRIMARY KEY (id_ingredient, id_recette) -- Un lien ingrédient/recette unique
);

-- Table de référence des catégories (ex: "Entrée", "Plat", "Dessert")
CREATE TABLE categories (
    id SERIAL PRIMARY KEY,           -- Identifiant unique de la catégorie
    nom VARCHAR(50) UNIQUE NOT NULL  -- Nom de la catégorie, unique
);

-- Table de jonction Recette <-> Catégories (relation N-N)
CREATE TABLE categories_recettes (
    id_categorie INT NOT NULL REFERENCES categories(id) ON DELETE CASCADE,
    -- Référence à la catégorie
    id_recette INT NOT NULL REFERENCES recette(id) ON DELETE CASCADE,
    -- Référence à la recette
    PRIMARY KEY (id_categorie, id_recette) -- Une liaison catégorie/recette unique
);

-----------------------------------------------------------
-- Données de test : 6 recettes (sans utilisateurs créateurs)
-----------------------------------------------------------
INSERT INTO recette (nom, temps_preparation, temps_cuisson, difficulte, photo, createur)
VALUES 
    ('nom_1','00:00:00','00:00:00',3,'photo_1', NULL),
    ('nom_2','00:00:00','00:00:00',2,'photo_1', NULL),
    ('nom_3','00:00:00','00:00:00',2,'photo_1', NULL),
    ('nom_4','00:00:00','00:00:00',5,'photo_1', NULL),
    ('nom_5','00:00:00','00:00:00',2,'photo_1', NULL),
    ('nom_6','00:00:00','00:00:00',3,'photo_1', NULL);

-- Validation de toutes les opérations de la transaction
COMMIT;
