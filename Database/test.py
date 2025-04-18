from crud import (
    insertUser, checkLogin, fetchQuestions,
    saveScore, getUserId, fetchCombinedQuestionsAndEvents
)

def test_all():
    print("\n🔐 Inserting test user...")
    inserted = insertUser("test_user", "supersecurepw")
    print("Inserted:", inserted)

    print("\n🔍 Checking login...")
    logged_in = checkLogin("test_user", "supersecurepw")
    print("Login success:", logged_in)

    print("\n🔢 Getting user ID...")
    uid = getUserId("test_user")
    print("User ID:", uid)

    print("\n🎯 Saving score...")
    saveScore(uid, 87)
    print("Score saved.")

    print("\n📚 Fetching combined questions and horror events...")
    combined = fetchCombinedQuestionsAndEvents()
    for item in combined:
        print(f"- [{item['type'].upper()}] {item['data']}\n")

if __name__ == "__main__":
    test_all()
