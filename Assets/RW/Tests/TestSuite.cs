using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
	// [Test] = simple method
	// [UnityTest] = coroutine

	private Game game;

	[SetUp]
	public void Setup()
	{
		GameObject gameGameObject =
			MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
		game = gameGameObject.GetComponent<Game>();
	}

	[TearDown]
	public void Teardown()
	{
		Object.Destroy(game.gameObject);
	}

	// 1
	[UnityTest]
	public IEnumerator AsteroidsMoveDown()
	{
		// 2
		// GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
		// Game game = gameGameObject.GetComponent<Game>();
		// 3
		GameObject asteroid = game.GetSpawner().SpawnAsteroid();
		// 4
		float initialYPos = asteroid.transform.position.y;
		// 5
		yield return new WaitForSeconds(0.1f);
		// 6
		Assert.Less(asteroid.transform.position.y, initialYPos);
		// 7
		// Object.Destroy(game.gameObject);
	}

	/*
		1) This is an attribute. Attributes define special compiler behaviors. It
		tells the Unity compiler that this is a unit test. This will make it appear in
		the Test Runner when you run your tests.

		2) Creates an instance of the Game. Everything is nested under the game, so when
		you create this, everything you need to test is here. In a production
		environment, you will likely not have everything living under a single prefab.
		So, you’ll need to take care to recreate all the objects needed in the scene.

		3) Here you are creating an asteroid so you can keep track of whether it moves. The
		SpawnAsteroid method returns an instance of a created asteroid. The Asteroid
		component has a Move method on it (feel free to look at the Asteroid script
		under RW / Scripts if you’re curious how the movement works).

		4) Keeping track of the initial position is required for the assertion where you
		verify if the asteroid has moved down.

		5) All Unity unit tests are coroutines, so you need to add a yield return. You’re
		also adding a time-step of 0.1 seconds to simulate the passage of time that the
		asteroid should be moving down. If you don’t need to simulate a time-step, you
		can return a null.

		6) This is the assertion step where you are asserting that the position of the
		asteroid is less than the initial position (which means it moved down).
		Understanding assertions is a key part of unit testing, and NUnit provides
		different assertion methods. Passing or failing the test is determined by this
		line.

		7) Your mom might not yell at you for leaving a mess after your unit tests are
		finished, but your other tests might decide to fail because of it. :[ It’s
		always critical that you clean up (delete or reset) your code after a unit test
		so that when the next test runs there are no artifacts that can affect that
		test. Deleting the game object is all you have left to do, since for each test
		you’re creating a whole new game instance for the next test.
	*/

	[UnityTest]
	public IEnumerator GameOverOccursOnAsteroidCollision()
	{
		// GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
		// Game game = gameGameObject.GetComponent<Game>();
		GameObject asteroid = game.GetSpawner().SpawnAsteroid();
		//1
		asteroid.transform.position = game.GetShip().transform.position;
		//2
		yield return new WaitForSeconds(0.1f);

		//3
		Assert.True(game.isGameOver);

		// Object.Destroy(game.gameObject);
	}

	/*
		1) You are forcing an asteroid and ship crash by explicitly setting the
		asteroid to have the same position as the ship. This will force their
		hitboxes to collide and cause game over. If you’re curious how that code
		works, look at the Ship, Game, and Asteroid files in the Scripts folder.

		2) A time-step is needed to ensure the Physics engine Collision event fires so a
		0.1 second wait is returned.

		3) This is a truth assertion, and it checks that the gameOver flag in the Game
		script has been set to true. The game code works with this flag being set to
		true when the ship is destroyed, so you’re testing to make sure this is set to
		true after the ship has been destroyed.
	*/

	[UnityTest]
	public IEnumerator NewGameRestartsGame()
	{
		//1
		game.isGameOver = true;
		game.NewGame();
		//2
		Assert.False(game.isGameOver);
		yield return null;
	}

	/*
		1) This part of the code sets up this test to have the gameOver bool set to
		true. When the NewGame method is called, it should set this flag back to
		false.

		2) Here, you assert that the isGameOver bool is false, which should be the case
		after a new game is called.
	*/

	[UnityTest]
	public IEnumerator LaserMovesUp()
	{
		// 1
		GameObject laser = game.GetShip().SpawnLaser();
		// 2
		float initialYPos = laser.transform.position.y;
		yield return new WaitForSeconds(0.1f);
		// 3
		Assert.Greater(laser.transform.position.y, initialYPos);
	}

	/*
		1) This gets a reference to a created laser spawned from the ship.

		2) The initial position is recored so you can verify that it’s moving up.

		3) This assertion is just like the one in the AsteroidsMoveDown unit test,
		only now you’re asserting that the value is greater (indicating that the
		laser is moving up).
	*/

	[UnityTest]
	public IEnumerator LaserDestroysAsteroid()
	{
		// 1
		GameObject asteroid = game.GetSpawner().SpawnAsteroid();
		asteroid.transform.position = Vector3.zero;
		GameObject laser = game.GetShip().SpawnLaser();
		laser.transform.position = Vector3.zero;
		yield return new WaitForSeconds(0.1f);
		// 2
		UnityEngine.Assertions.Assert.IsNull(asteroid);
	}

	/*
		1) You are creating an asteroid and a laser, and making sure they have
		the same position so as to trigger a collision.

		2) A special test with an important distinction. Notice how you are
		explicitly using UnityEngine.Assertions for this test? That’s because
		Unity has a special Null class which is different from a “normal” Null
		class. The NUnit framework assertion Assert.IsNull() will not work for
		Unity null checks. When checking for nulls in Unity, you must explicitly
		use the UnityEngine.Assertions.Assert, not the NUnit Assert.
	*/

	[UnityTest]
	public IEnumerator DestroyedAsteroidRaisesScore()
	{
		// 1
		GameObject asteroid = game.GetSpawner().SpawnAsteroid();
		asteroid.transform.position = Vector3.zero;
		GameObject laser = game.GetShip().SpawnLaser();
		laser.transform.position = Vector3.zero;
		yield return new WaitForSeconds(0.1f);
		// 2
		Assert.AreEqual(game.score, 1);
	}

	/*
		1) You’re spawning an asteroid and a laser, and making sure they’re in the
		same position. This ensures a collision occurs, which will trigger a
		score increase.

		2) This asserts that the game.score int is now 1 (instead of the 0 that it
		starts at).
	*/

}
