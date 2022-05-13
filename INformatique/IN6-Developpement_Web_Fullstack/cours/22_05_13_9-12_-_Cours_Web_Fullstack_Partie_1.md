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

    Vu en TP

    4. Zoom sur le back et le protocole HTTP  

    Méthodes HTTP : Liste exhaustive.
    Se focus sur les ressources
    Différence POST et PUT... question de parti pris.
    Patch rare

    Ressource = tout type de donnée contenu dans un site.  

    Codes de retour :  
    * 1XX : Retour d'information  
    * 2XX : Succès  
        * 200 : OK  
        * 204 : No Content  
    * 3XX : Redirection  
    * 4XX : Erreur Client  
        * 400 : Bad Request  
        * 401 : Unauthorized  
        * 403 : Forbidden  
        * 404 : Not Found  
    * 5XX : Erreur serveur  
        * 500 : Internal Server Error  

    5. REST bonnes pratiques  

    "Différence POST et PUT... question de parti pris." : Pas d'obligation, n'est là que pour guider l'implémentation. MAIS question de cohérence, d'où le besoin d'un concensus.  

    REST : REpresentational State Transfer  
    * Découpage clair client/serveur  
    * Interface uniforme  
    * Sans état  
    * Permettre les architectures en couches  
    * Expliciter la mise en cache  

    RESTful : qui implémente toutes les bonnes pratiques

    ## Cas concrets  

    * Ressources en kebab-case  
    * Ne pas dénaturer l'utilisation des méthodes HTTP (tip: un verbe dans l'URL ? Mauvaise idée)  
    * Ne pas dénaturer l'utilisation des codes HTTP...  
    * ... Et fournir les détails en cas d'erreur client ! "Qu'est-ce qui ne va pas, **exactement** ?"
    * Ressources nommées au pluriel  

2. Considérations méthodologiques  

    1. Gestion des sources  
    Historique, principales commandes Git  
    Forge Logicielle (~chaîne DevOps)  
    * Un (ou plusieurs) gestionnaires de versions  
    * Gestion des tâches  
    * Outils de suivi d'anomalies  
    * Gestion documentaire  
    * Intégration et Déploiements automatisés (CI/CD)  

    2. Qualité des test  
    
    Comment assurer la qualité ?  
    * Ne pas se lancer dans le développement la tête baissée.  
        Prendre un café avant de coder, y réfléchir **avant**.  
    * Anticiper le fonctionnel  
        Le penser. Savoir ce qu'on veut.  
    * Prévoir une architecture maintenable
        Même en considérant l'absence de bug, faire en sorte que l'évolution soit aisée **pour les autres**.  
        Clair, simple. *Keep it simple and stupid*  
        * Gérer les cas d'erreurs, TOUS AUTANT QUE POSSIBLE, et en étant **VERBEUX**.  
        ![V](https://www.youtube.com/watch?v=voLrkal39C4)  
    * Gérer les cas d'erreurs    
        * Toujours penser à gérer les cas d'erreur dans le code    
            * Bien gérer les retours HTTP    
            * User des try/catch    
        * Même si l'on n'est pas censé le rencontrer !
            * Ne pas hésiter à lever des exceptions
            * Anticiper les erreurs de type *Null Reference*
        * Toujours bien détailler les rapports d'erreur !
            * Faire des phrases !
            * Classifier autant que possible les erreurs
    * Tester et automatiser    
        * Tests unitaires  
            * Vérifier une fonction/un programme/un endpoint  
            * Couvrir les cas nominaux  
            * Couvrir les cas d'erreur  
        * Tests fonctionnels  
            * Vérifier le fonctionnement général  
            * Couvrir des scénarios complets, si possible de bout-en-bout  
            * Être le plus proche possible des conditions réelles d'utilisation finale  
            * Nécessite des jeux de données complets  
        * Mais comment tester ?  
            * À l'ancienne : faire des cahiers de test à la main  
            * À l'arrache : tester c'est douter  
            * Automatisés : les tests sont programmés  

    3. Méthodologie TDD  
    * "Test Driven Development"  
    * Concevoir les tests **avant** le code  
    * Nécessite de se poser les bonnes questions avant de coder  
    * Peut être considéré comme une spéc  
        * Test Unitaires : TDD, "Test Driven Development"  
        * Test Fonctionnel : BDD, "Behavior Driven Development"  
    * Avant : tout rouge ; Après : tout vert  
    * TL;DR : Cycle général :  
        * Codeur  
        * Source  
        * Tests  
        * Conformité ?  
            * Oui : Commit  
            * Non : Loop  
