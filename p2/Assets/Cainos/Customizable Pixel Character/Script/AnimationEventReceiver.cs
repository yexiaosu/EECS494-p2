using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        private PixelCharacterController controller;

        private void Awake()
        {
            controller = GetComponentInParent<PixelCharacterController>();
        }

        public void OnFootstep(AnimationEvent evt)
        {
            controller.OnFootstep(evt);
        }


        #region - ATTACK EVENTS

        public void OnAttackStart()
        {
            controller.OnAttackStart();
        }
        public void OnAttackHit()
        {
            controller.OnAttackHit();
        }
        public void OnAttackEnd()
        {
            controller.OnAttackEnd();
        }

        public void OnThrow()
        {
            controller.OnThrow();
        }

        #endregion

        #region - ARCHERY EVENTS -

        public void OnArrowDraw()
        {
            if (controller) controller.OnArrowDraw();
        }

        public void OnArrowNock()
        {
            if (controller) controller.OnArrowNock();
        }

        public void OnArrowReady()
        {
            if (controller) controller.OnArrowReady();
        }

        public void OnArrowPutBack()
        {
            if (controller) controller.OnArrowPutBack();
        }

        #endregion

        #region - LEDGE CLIMB EVENTS - 

        //when ledge climb animation passes the time this event defines
        //it cannot be cancelled by releasing forward key
        public void OnLedgeClimbLocked()
        {
            if (controller) controller.OnLedgeClimbLocked();
        }

        public void OnLedgeClimbFinised()
        {
            if (controller) controller.OnLedgeClimbFinised();
        }

        #endregion

        #region - LADDER CLIMB EVENTS -

        public void OnLadderEntered()
        {
            if (controller) controller.OnLadderEntered();
        }

        public void OnLadderExited()
        {
            if (controller) controller.OnLadderExited();
        }

        #endregion

        #region - CRAWL EVENTS -

        public void OnCrawlEnter()
        {
            if (controller) controller.OnCrawlEnter();
        }
        public void OnCrawlEntered()
        {
            if (controller) controller.OnCrawlEntered();
        }
        public void OnCrawlExit()
        {
            if (controller) controller.OnCrawlExit();
        }
        public void OnCrawlExited()
        {
            if (controller) controller.OnCrawlExited();
        }



        #endregion

        #region - DODGE EVENTS -
        private void OnDodgeStart()
        {
            controller.OnDodgeStart();
        }

        private void OnDodgeEnd()
        {
            controller.OnDodgeEnd();
        }

        #endregion
    }
}
