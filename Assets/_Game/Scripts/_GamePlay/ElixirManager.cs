using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Yuddham
{
    public class ElixirManager : MonoBehaviour
    {
        private List<Image> _elixirList = new List<Image>();

        private List<Image> _usableElixirs = new List<Image>();

        private const float ElixirFillTime = 0.5f;
        private float _elixirFillMultiplier = 1f;

        private void Awake()
        {
            _elixirList = GetComponentsInChildren<Image>().ToList();
            _usableElixirs = new List<Image>();
            Sequence init = DOTween.Sequence();
            foreach (var image in _elixirList)
            {
                image.fillMethod = Image.FillMethod.Horizontal;
                image.fillAmount = 0;
                init.Append(ElixirFillSequence(image));
            }
            init.Play();
            _elixirFillMultiplier = 1;
        }

        private Sequence ElixirFillSequence(Image elixir)
        {
            Sequence seq = DOTween.Sequence().Append(DOTween.To(() => elixir.fillAmount, x => elixir.fillAmount = x, 1,
                ElixirFillTime * _elixirFillMultiplier));
            seq.onComplete = () =>
            {
                elixir.rectTransform.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
                _usableElixirs.Add(elixir);
            };
            return seq;
        }

        public void UseElixir(int count)
        {
            Image elixir = _usableElixirs[0];
            for (int i = 0; i < count; i++)
            {
                elixir = _usableElixirs[i];
                elixir.fillAmount = 0;
                if (_elixirList.Contains(elixir))
                {
                    elixir.transform.SetAsLastSibling();
                }

                _usableElixirs.Remove(elixir);
            }
        }
    }
}