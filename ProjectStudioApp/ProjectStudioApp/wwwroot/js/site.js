// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', () => {
    // 触发按钮：Conservation 页面底部的黄色按钮 ID
    const openBtn = document.getElementById('openDonateModalBottom');

    // 模态窗口和步骤元素
    const donateModal = document.getElementById('donateModal');
    // 注意：这里使用 querySelectorAll 获取所有具有 close-btn 类的元素
    const closeBtns = document.querySelectorAll('.close-btn');
    const goToPaymentBtn = document.getElementById('goToPayment');
    const paymentForm = document.getElementById('paymentForm');
    const amountBtns = document.querySelectorAll('.amount-btn');
    const customAmountInput = document.getElementById('customAmount');

    let selectedAmount = 0; // 存储选定的金额

    // --- 模态窗口显示/隐藏 ---
    // 只有当按钮存在于当前页面时才绑定事件
    if (openBtn) {
        openBtn.onclick = () => {
            donateModal.style.display = 'block';
            showStep(1); // 每次打开都从第一步开始
        }
    }

    closeBtns.forEach(btn => {
        btn.onclick = () => {
            donateModal.style.display = 'none';
        }
    });

    // 点击窗口外部关闭模态窗口
    window.onclick = (event) => {
        if (event.target == donateModal) {
            donateModal.style.display = 'none';
        }
    }

    // --- 步骤切换函数 ---
    const showStep = (stepNumber) => {
        const steps = document.querySelectorAll('.donate-step');
        steps.forEach(step => step.classList.remove('active'));
        document.getElementById(`donate-step-${stepNumber}`).classList.add('active');

        // 更新标题和金额显示
        const titleElement = document.getElementById('donate-title');
        if (stepNumber === 1) {
            titleElement.textContent = 'Step 1: Choose Your Amount';
        } else if (stepNumber === 2) {
            titleElement.textContent = 'Step 2: Payment Details';
            // 更新金额显示，保留两位小数
            document.getElementById('finalAmountDisplay').textContent = `$${selectedAmount.toFixed(2)}`;
        } else if (stepNumber === 3) {
            titleElement.textContent = 'Donation Complete!';
            document.getElementById('thankYouAmount').textContent = `$${selectedAmount.toFixed(2)}`;
        }
    }

    // --- 金额选择逻辑 (Step 1) ---
    const updateSelectedAmount = (amount) => {
        // 确保金额为数字并至少为0
        selectedAmount = Math.max(0, parseFloat(amount) || 0);
        goToPaymentBtn.disabled = selectedAmount <= 0;
    };

    amountBtns.forEach(btn => {
        btn.onclick = () => {
            // 样式切换
            amountBtns.forEach(b => b.classList.remove('selected'));
            btn.classList.add('selected');
            customAmountInput.value = '';

            updateSelectedAmount(parseFloat(btn.dataset.amount));
        };
    });

    customAmountInput.oninput = () => {
        amountBtns.forEach(b => b.classList.remove('selected'));
        updateSelectedAmount(customAmountInput.value);
    };

    // --- 前往支付按钮 (Step 1 -> 2) ---
    goToPaymentBtn.onclick = () => {
        if (selectedAmount > 0) {
            showStep(2);
        }
    };

    // --- 支付表单提交 (Step 2 -> 3 模拟) ---
    paymentForm.onsubmit = (e) => {
        e.preventDefault();

        // --- 模拟支付成功 ---
        // 禁用按钮并显示处理中状态
        const completeBtn = document.getElementById('completeDonation');
        completeBtn.textContent = 'Processing...';
        completeBtn.disabled = true;

        setTimeout(() => {
            // 恢复按钮状态
            completeBtn.textContent = 'Complete Donation';
            completeBtn.disabled = false;

            // 模拟成功，切换到感谢页面
            showStep(3);
        }, 1500); // 模拟支付处理延迟 1.5 秒
    };
});