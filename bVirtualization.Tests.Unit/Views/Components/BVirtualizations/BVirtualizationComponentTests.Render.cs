﻿// ---------------------------------------------------------------
// Copyright (c) Brian Parker & Hassan Habib All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Linq;
using Bunit;
using bVirtualization.Models.BVirutalizationComponents;
using bVirtualization.Views.Components;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Moq;
using Xunit;

namespace bVirtualization.Tests.Unit.Views.Components.BVirtualizations
{
    public partial class BVirtualizationComponentTests
    {
        [Fact]
        public void ShouldInitializeComponent()
        {
            // given
            BVirutalizationComponentState expectedState =
                BVirutalizationComponentState.Loading;

            // when
            var initialBVirtualizationComponent =
                new BVirtualizationComponent<object>();

            // then
            initialBVirtualizationComponent.State.Should().Be(expectedState);
            initialBVirtualizationComponent.VirtualizeService.Should().BeNull();
            initialBVirtualizationComponent.ChildContent.Should().BeNull();
            initialBVirtualizationComponent.Label.Should().BeNull();
            initialBVirtualizationComponent.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public void ShouldRenderContent()
        {
            // given
            BVirutalizationComponentState expectedState =
                BVirutalizationComponentState.Content;

            IQueryable<object> randomData =
                CreateRandomQueryable();

            IQueryable<object> retrievedData =
                randomData;

            IQueryable<object> expectedData =
                retrievedData;

            RenderFragment<object> inputChildContent = 
                CreateRenderFragment(typeof(SomeComponent<object>));

            RenderFragment<object> expectedChildContent =
                inputChildContent;

            ComponentParameter parameter =
                ComponentParameter.CreateParameter(
                    nameof(BVirtualizationComponent<object>.ChildContent),
                    inputChildContent);
            
            this.virtualizationServiceMock.Setup(service =>
                service.LoadPage(It.IsAny<uint>(), It.IsAny<uint>()))
                    .Returns(retrievedData);

            // when
            this.renderedComponent = 
                RenderComponent<BVirtualizationComponent<object>>(parameter);

            // then
            this.renderedComponent.Instance.State
                .Should().Be(expectedState);

            this.renderedComponent.Instance.ChildContent
                .Should().BeEquivalentTo(expectedChildContent);

            this.virtualizationServiceMock.Verify(service =>
                service.LoadPage(It.IsAny<uint>(), It.IsAny<uint>()),
                    Times.Once);

            this.renderedComponent.Instance.ErrorMessage.Should().BeNull();
            this.renderedComponent.Instance.Label.Should().BeNull();
            this.virtualizationServiceMock.VerifyNoOtherCalls();
        }
    }
}
