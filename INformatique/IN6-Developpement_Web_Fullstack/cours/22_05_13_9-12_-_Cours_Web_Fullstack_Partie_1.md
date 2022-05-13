# Eval

Projet :
* App web
* Quizz
* En trinôme
* Sur notre matériel
* Accent sur le travail personnel

# Séances

Cours 1 :
* Généralités
* Partie back-end

TP 1 : mise en place du back

Cours et TP 2 : Front

Cours 3 : Évaluation

# Cours 1

1. Considérations techniques  
    1. Qu'est-ce qu'une application web ?  
    
    IHM accessible via nav. web
        Via norme et protocoles HTTP(S), HTML et Scripts
    Dispo via réseau
    Contient un ensemble de libs
    Exécuté sur un ensemble de serveurs

    Distinction client léger - client lourd :
        Site web vs appli installée
        Progressive WebApp vs Appli Native
    
    Architecture Logique MVC
        ...
    
    Architecture matérielle 3 tiers Web
        Client --> HTTP - HTML <-- Serveur applicatif -> SQL - Result <-- Serveur de BDD

        Equivaut à Vue - Controlleur - Modèle, mais sans interaction Modèle - Vue
    
    2. Histoire de l'architecture web  

    Les débuts : Exemple Stack 1.0
        IE - Apache + PHP - PostGre
    POO : Exemple Stack Web OOP
        Années 2000
        Firefox - Apache/JSP/Java/Hibernate - MySQL
        HTML complété par JS
        Hibernate : Un Object Relational Mapping (ORM)
        Mixité des couches et du modèle MVC ==> Oops !
    2005 : Ajout d'interactions dynamiques, Exemple Stack Web 2.0
        Chrome - MiscrosoftIIS/ASPX/.NET MVC ASP.NET/Entity Framework - SQLServer
        Ajout des requêtes AJAX au HTML/JS (Réponse en XML)
    2015 : Arrivée des frameworks Javascript, Exemple Stack JS
        AngularJS - NodeJS/Expresss - mongoDB
    API Rest + VueJS, Python/Flask et SQLite

    3. Choix d'aujourd'hui Architecture API REST / SPA  
    4. Zoom sur le back et le protocole HTTP  
    5. REST bonnes pratiques  

2. Considérations méthodo  
    1. Gestion des src  
    2. Q des test  
    3. Méthodologie TDD