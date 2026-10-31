# Red–Green–Refactor w TDD i pracy z AI

## Cel

**Red–Green–Refactor** to krótki cykl Test-Driven Development:

1. **Red** — napisz mały test opisujący oczekiwane zachowanie i potwierdź, że nie przechodzi.
2. **Green** — dodaj najmniejszą implementację powodującą przejście testu.
3. **Refactor** — popraw strukturę bez zmiany zachowania; testy muszą pozostać zielone.

```text
Lista zachowań → RED → GREEN → REFACTOR → następne zachowanie
```

---

## Zalecenia Roberta C. Martina

### Trzy prawa TDD

Robert C. Martin opisuje nanocykl TDD za pomocą trzech praw:

1. Nie pisz kodu produkcyjnego, zanim nie powstanie test, który nie przechodzi.
2. Nie pisz więcej testu, niż potrzeba, aby test nie przeszedł. Brak kompilacji również jest niepowodzeniem.
3. Nie pisz więcej kodu produkcyjnego, niż potrzeba, aby aktualnie nieprzechodzący test przeszedł.

Martin odróżnia ten nanocykl od mikrocyklu Red–Green–Refactor:

- utwórz nieprzechodzący test jednostkowy,
- napisz kod powodujący jego przejście,
- posprzątaj powstały kod.

Uzasadnieniem rozdzielenia faz są dwa różne cele: **poprawne zachowanie** i **poprawna struktura**. Najpierw doprowadzamy zachowanie do poprawności, a następnie poprawiamy strukturę działającego rozwiązania.

Źródło: [Robert C. Martin — The Cycles of TDD](http://blog.cleancoder.com/uncle-bob/2014/12/17/TheCyclesOfTDD.html)

---

## Zweryfikowane odniesienie do programowania z AI

W serii **Clean AI: Agentic Discipline**, prowadzonej przez Justina Martina i Roberta C. Martina, jako główne zagrożenie przy używaniu agentów AI wskazano **niezdyscyplinowane podejście**. Publiczny opis pierwszego odcinka mówi, że omawiane są podstawowe dyscypliny **testowania i refaktoryzacji** podczas pracy z agentem takim jak Claude Code. Materiał wykorzystuje trzy studia przypadku, obejmujące kod tworzony od początku i modernizację starego kodu.

Źródło: [Clean Coders — Clean AI: Agentic Discipline, Episode 1](https://cleancoders.com/episode/agentic-discipline-1)

> Publiczny opis potwierdza kierunek rekomendacji, ale nie udostępnia całej procedury z płatnego nagrania. Workflow poniżej jest praktyczną adaptacją opublikowanych zasad Martina, a nie jego dosłownym cytatem.

---

## Praktyczny workflow z agentem AI

```text
Człowiek wybiera jedno zachowanie
        ↓
RED: powstaje test i zostaje uruchomiony
        ↓
Człowiek potwierdza właściwą przyczynę niepowodzenia
        ↓
GREEN: AI dodaje wyłącznie kod potrzebny do przejścia testu
        ↓
Uruchomienie odpowiedniego zestawu testów
        ↓
REFACTOR: AI poprawia strukturę bez zmiany zachowania
        ↓
Ponowne testy i przegląd zmian przez człowieka
        ↓
Następne małe zachowanie
```

### RED z AI

Agent może zaproponować test, ale przed implementacją człowiek powinien sprawdzić:

- czy test reprezentuje wymagane zachowanie,
- czy sprawdza obserwowalny rezultat zamiast przypadkowego szczegółu implementacji,
- czy został rzeczywiście uruchomiony,
- czy nie przechodzi z oczekiwanego powodu,
- czy nie został dopasowany do implementacji planowanej przez agenta.

Przykładowa instrukcja:

```text
Dodaj jeden test dla opisanego zachowania. Nie zmieniaj jeszcze kodu
produkcyjnego. Uruchom test i przedstaw przyczynę niepowodzenia.
Zatrzymaj się po potwierdzeniu fazy RED.
```

### GREEN z AI

Agent powinien wprowadzić tylko zmianę potrzebną dla aktualnego testu.

```text
Wprowadź najmniejszą zmianę w kodzie produkcyjnym, która spowoduje
przejście tego testu. Nie implementuj kolejnych przypadków, nie dodawaj
abstrakcji na przyszłość i nie osłabiaj testu. Uruchom odpowiedni zestaw
testów i przedstaw wynik.
```

Do zaakceptowania fazy GREEN potrzebny jest wynik wykonania testów, nie samo zapewnienie agenta.

### REFACTOR z AI

Po uzyskaniu zielonego stanu agent może przeanalizować:

- duplikację,
- nazewnictwo,
- rozdział odpowiedzialności,
- złożoność warunków,
- czytelność testów,
- zbędne zależności i abstrakcje.

```text
Testy są zielone. Zaproponuj mały refaktoring poprawiający strukturę bez
zmiany zachowania. Wprowadź jedną logiczną zmianę, uruchom testy i opisz,
co zostało uproszczone. Jeśli kod jest wystarczająco prosty, nie twórz
nowej abstrakcji.
```

---

## Guardraile dla agenta

- **Jedno zachowanie na iterację.** Nie przekazuj całego backlogu w jednym przebiegu.
- **Wymagaj rzeczywistego RED.** Test przechodzący od początku nie dowodzi, że wykrywa brak implementacji.
- **Nie zmieniaj testu i implementacji jednocześnie w GREEN.** Agent mógłby dopasować oba elementy do siebie i ominąć wymaganie.
- **Wymagaj dowodu z narzędzia.** Potrzebny jest wynik kompilacji i testów.
- **Uruchamiaj regresję.** Przejście nowego testu nie wystarcza, jeśli wcześniejsze zachowanie zostało uszkodzone.
- **Zachowaj human checkpoint.** Człowiek zatwierdza znaczenie testu, zakres implementacji i refaktoring.
- **Oddziel poprawność od struktury.** Najpierw zachowanie, potem projekt.
- **Nie pozwalaj agentowi osłabiać asercji.** Test jest ograniczeniem, a nie przeszkodą do usunięcia.

---

## Przykład w C# i xUnit

### RED

```csharp
public sealed class DiscountCalculatorTests
{
    [Fact]
    public void Apply_TenPercentDiscount_ReducesPriceByTenPercent()
    {
        var calculator = new DiscountCalculator();

        decimal result = calculator.Apply(100m, 10m);

        Assert.Equal(90m, result);
    }
}
```

Uruchom test i potwierdź właściwą przyczynę błędu:

```bash
dotnet test
```

### GREEN

```csharp
public sealed class DiscountCalculator
{
    public decimal Apply(decimal price, decimal discountPercent)
    {
        return price * (1m - discountPercent / 100m);
    }
}
```

Ponownie uruchom wszystkie właściwe testy:

```bash
dotnet test
```

### REFACTOR

Jeżeli implementacja jest już mała, czytelna i pozbawiona duplikacji, poprawnym wynikiem fazy REFACTOR może być **brak zmian**. Refaktoring nie jest obowiązkiem tworzenia nowej abstrakcji.

---

## Checklista iteracji

### RED

- [ ] Wybrano jedno małe zachowanie.
- [ ] Powstał test opisujący to zachowanie.
- [ ] Test został uruchomiony.
- [ ] Test nie przeszedł z oczekiwanego powodu.
- [ ] Człowiek zatwierdził znaczenie testu.

### GREEN

- [ ] Dodano minimalną implementację.
- [ ] Agent nie osłabił ani nie usunął testu.
- [ ] Nowy test przechodzi.
- [ ] Wcześniejsze testy nadal przechodzą.
- [ ] Wyniki pochodzą z rzeczywistego uruchomienia narzędzia.

### REFACTOR

- [ ] Zbadano duplikację i czytelność.
- [ ] Nie dodano nowego zachowania.
- [ ] Nie utworzono abstrakcji bez uzasadnionej potrzeby.
- [ ] Po zmianach cały właściwy zestaw testów pozostaje zielony.
- [ ] Człowiek przejrzał finalny diff.

---

## Ograniczenie dowodów

TDD zapewnia agentowi mierzalne ograniczenia i mechanizm wykrywania regresji, ale samo polecenie stosowania TDD nie gwarantuje lepszego rozwiązania. Eksploracyjny eksperyment opisany przez Birgittę Böckeler nie wykazał wyraźnej przewagi jakościowej rozwiązań tworzonych przez agenta w trybie TDD nad rozwiązaniami bez takiego workflow. Autorka nie przedstawia go jako rozstrzygającej ewaluacji.

Wniosek praktyczny: ważne są nie tylko etykiety RED, GREEN i REFACTOR, lecz także jakość testów, rzeczywiste wykonanie narzędzi, mała skala zmian, checkpointy i niezależna kontrola wyniku.

Źródło uzupełniające: [Birgitta Böckeler — TDD inside the agent loop](https://martinfowler.com/articles/exploring-gen-ai/tdd-in-the-agent-loop.html)

---

## Skrót

> **RED:** test opisuje brakujące zachowanie i nie przechodzi z właściwego powodu.  
> **GREEN:** AI dodaje najmniejszą potrzebną zmianę.  
> **REFACTOR:** AI poprawia strukturę przy zielonych testach.  
> **VERIFY:** człowiek sprawdza test, wynik narzędzi i diff.  
> **REPEAT:** rozpoczyna się następna mała iteracja.

## Źródła

- [Robert C. Martin — The Cycles of TDD](http://blog.cleancoder.com/uncle-bob/2014/12/17/TheCyclesOfTDD.html)
- [Clean Coders — Clean AI: Agentic Discipline, Episode 1](https://cleancoders.com/episode/agentic-discipline-1)
- [Martin Fowler — Test Driven Development](https://martinfowler.com/bliki/TestDrivenDevelopment.html)
- [Birgitta Böckeler — TDD inside the agent loop](https://martinfowler.com/articles/exploring-gen-ai/tdd-in-the-agent-loop.html)
