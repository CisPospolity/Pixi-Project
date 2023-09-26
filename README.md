# Pixi-Project

## Klonowanie repo

1. Czego używamy:
   - Unity 2022.3.10f1
   - Program do zarządzania gitem jest dowolny (to co pozwala na graficzne dodawanie commitów zamiast używania konsolki). Ja używam Sourcetree ale wybór dowolny nawet Github Desktop


Główne branche:
main - wersja stabilna / gotowa do buildu
develop - wersja rozwojowa / tutaj wrzucamy działające zmiany

Przy rozpoczęciu pracy nad zmianą (czy to kod, dodanie modelu, edycja sceny) tworzymy nowego brancha.
Po skończonej pracy tworzycie "merge request" z waszego brancha do developa, a ja to potem łączę.
Jeśli na developie jest dużo nowych zmian, albo zmiany związane z tym co robicie na branchu to polecam sobię zmergować develop do waszego brancha, aby potem nie było konfliktów.

Prefiksy branchy:
"feature/" - dodajemy nową funkcjonalność (głownie kod)
"asset/" - dodajemy same modele, grafiki, animacje i inne assety bez funkcjonalności
"design/" - tworzenie UI, poziomów
