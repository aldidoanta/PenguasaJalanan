//Player.cs

using UnityEngine;
using System.Collections;

//Handles player behavior
public class Player : MonoBehaviour
{

    //player number
    public int playerNumber;

    //player movement and physics
    private float moveForce = 10.0f;
    private float maxSpeed = 4.0f;

    private float jumpForce = 390.0f;
    private float fallTime = 0.5f;

    private bool isPhased = false;

    //player attack
    private int maxAttack = 3; //maximum number of consecutive attacks allowed for the player
    [HideInInspector] public int attackRemaining = 3; //number of consecutive attacks remaining for the player
    private float attackTime;

    //player knockback
    [HideInInspector] public float knockbackPoint = 0f;
    private float knockbackForce = 50.0f;
    [HideInInspector] public Vector2 knockbackDirection = Vector2.zero;

    //player animation
    [HideInInspector] public Animator playerAnim;
    public AnimationClip jumpprepAnim;
    public AnimationClip fallplatformAnim;
    public AnimationClip knockbackAnim;
    public bool isFacingRight = true;

    //used for player control
    [HideInInspector] public float inputX = 0f;
    [HideInInspector] public bool grounded = false;
    [HideInInspector] public bool isJumping = false;
    public Transform feetCheck;
    public Transform groundCheck;

    private AudioPlayer audioplayer;

    void Awake()
    {
        playerAnim = GetComponent<Animator>();
        if (playerAnim == null)
            Debug.Log("Player animation not found");
    }

    // Use this for initialization
    void Start()
    {
        audioplayer = GameObject.Find("TutorialManager").GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

        /* JUMP */
        // The player is grounded if a linecast to the groundcheck position hits anything on the "Platform" or "Player" layer.
        grounded = ((Physics2D.Linecast(feetCheck.position, groundCheck.position, 1 << LayerMask.NameToLayer("Platform")))
                    || (Physics2D.Linecast(feetCheck.position, groundCheck.position, 1 << LayerMask.NameToLayer("Item"))));


        /* KNOCKBACK */
        if (knockbackDirection != Vector2.zero)
        {
            StartCoroutine(knockback(knockbackAnim));
        }

        /* ANIMATION */
        if (((inputX > 0) && (!isFacingRight)) || ((inputX < 0) && (isFacingRight)))//sprite mirrorring
        {
            flipSprite();
        }
        //set animator parameters
        playerAnim.SetBool("isFacingRight", isFacingRight);
    }

    //update physics
    void FixedUpdate()
    {
        /* MOVEMENT */
        //if player is changing direction, or has not reached maxSpeed
        if (inputX * rigidbody2D.velocity.x < maxSpeed)
        {
            //add force to player
            rigidbody2D.AddForce(new Vector2(moveForce * inputX, 0));
        }
        //if current x velocity exceeds maxSpeed
        if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
        {
            //limit the current velocity to maxSpeed
            rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
        }

        /* JUMP */

        //platform jump-through handling
        if ((rigidbody2D.velocity.y > 0) || (isPhased == true))
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerPhased"); //player will be able to jump through the platform
            feetCheck.gameObject.layer = LayerMask.NameToLayer("PlayerPhased"); //apply the same to feetCheck gameobject because of its collider
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            feetCheck.gameObject.layer = LayerMask.NameToLayer("Player");
        }

        //set animator parameters
        playerAnim.SetFloat("speedX", rigidbody2D.velocity.x);
        playerAnim.SetFloat("speedY", rigidbody2D.velocity.y);
    }

    /* ADDITIONAL METHODS */

    //used to flip sprite horizontally
    void flipSprite()
    {
        isFacingRight = !isFacingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; //flip horizontally
        transform.localScale = newScale;
    }

    //checks if current attack is followed by another attack or not (called after one attack animation has finished)
    //used by Player Animation
    void checkAttack(int currentAttackRemaining)
    {
        playerAnim.SetInteger("attackRemaining", currentAttackRemaining); //assign attackRemaining to animator's state
        if (attackRemaining == currentAttackRemaining)
        {
            playerAnim.SetBool("isAttacking", false); //finish the attack combo
            playerAnim.SetInteger("attackRemaining", maxAttack); //reset attackRemaining variable in animator
            attackRemaining = maxAttack; //reset attackRemaining (required for PlayerControl)
        }
    }

    //plays attack sound at given timing
    //used by Player Animation
    void playAttackSound()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.ATTACK]);//play the sound
    }

    //plays footstep sound at given timing
    //used by Player Animation
    void playWalkSound()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.WALK]);//play the sound
    }

    //plays jumping sound at given timing
    //used by Player Animation
    void playJumpSound()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.JUMP]);//play the sound
    }

    //plays falling from platform sound at given timing
    //used by Player Animation
    void playFallPlatformSound()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.FALLPLATFORM]);//play the sound
    }

    /* COROUTINES */

    public IEnumerator jump(AnimationClip anim)
    {
        isJumping = true;

        playerAnim.SetBool("isJumpingPrepare", true);
        //wait until "jump preparation" animation finished
        yield return new WaitForSeconds(anim.length);
        playerAnim.SetBool("isJumpingPrepare", false);

        // Add a vertical force to the player.
        rigidbody2D.AddForce(new Vector2(0f, jumpForce)); //this is the actual jump

        isJumping = false;
    }

    //make the player fall through the platform
    public IEnumerator fall()
    {
        playerAnim.SetBool("isFallingPlatform", true);
        isPhased = true;
        yield return new WaitForSeconds(fallTime);
        isPhased = false;
        playerAnim.SetBool("isFallingPlatform", false);
    }

    //knockback action
    IEnumerator knockback(AnimationClip anim)
    {
        playerAnim.SetBool("isKnockback", true);
        playerAnim.SetInteger("attackRemaining", maxAttack); //reset number of available attacks, otherwise the player may not be able to attack anymore

        audioplayer.playSound(audioplayer.sounds[audioplayer.HIT]);//play the sound

        knockbackPoint += knockbackForce;
        rigidbody2D.AddForce(new Vector2(knockbackPoint * knockbackDirection.x, knockbackPoint * knockbackDirection.y));
        //reset knockbackDirection
        knockbackDirection = Vector2.zero;

        yield return new WaitForSeconds(anim.length);

        //reset player animation
        playerAnim.SetBool("isKnockback", false);
    }

}
